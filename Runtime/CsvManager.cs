// --------------------------------------------------
// Author:      Takayoshi Hagiwara (NITech)
// Created:     2026/7/15
// Summary:     Reads and writes avatar motion data in CSV format.
// --------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TH.Utils.Avatar
{
    /// <summary>
    /// Provides CSV serialization and deserialization for avatar motion data.
    /// </summary>
    public static class CsvManager
    {
        private const string CsvExtension = ".csv";
        private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;
        private static readonly Encoding Utf8Encoding = new UTF8Encoding(false);

        private const string MotionDataHeader = "frameIndex,boneName,time," +
                                                "posX,posY,posZ," +
                                                "rotX,rotY,rotZ,rotW";
        private static readonly int ColumnCount = MotionDataHeader.Split(",").Length;

        /// <summary>
        /// Writes avatar motion data to a CSV file.
        /// Existing files with the same name are overwritten.
        /// </summary>
        /// <param name="motionData">The avatar motion data to write.</param>
        /// <param name="dataPath">The output directory path relative to <see cref="Application.dataPath"/>.</param>
        /// <param name="fileName">The output file name, with or without the CSV extension.</param>
        /// <returns>The absolute path of the written CSV file.</returns>
        public static string WriteMotionData(AvatarMotionData motionData, string dataPath, string fileName)
        {
            // Check motion data
            if (motionData == null)
                throw new ArgumentNullException(nameof(motionData));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("The file name must not be null or empty.", nameof(fileName));

            ValidateMotionData(motionData);

            // Directory settings
            string directoryPath = GetDirectoryPath(dataPath);
            Directory.CreateDirectory(directoryPath);

            string normalizedFileName = Path.GetFileNameWithoutExtension(fileName) + CsvExtension;
            string filePath = Path.Combine(directoryPath, normalizedFileName);

            using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.Read);
            using StreamWriter writer = new(fileStream, Utf8Encoding);

            // Write header
            writer.WriteLine(MotionDataHeader);

            StringBuilder row = new();

            // Write motion data
            foreach (KeyValuePair<string, List<Vector3>> positionEntry in motionData.Positions)
            {
                string boneName = positionEntry.Key;
                List<Vector3> positions = positionEntry.Value;
                List<Quaternion> rotations = motionData.Rotations[boneName];

                for (int frameIndex = 0; frameIndex < motionData.FrameCount; frameIndex++)
                {
                    WriteMotionRow(
                        row,
                        writer,
                        frameIndex,
                        boneName,
                        motionData.Times[frameIndex],
                        positions[frameIndex],
                        rotations[frameIndex]);
                }
            }

            return filePath;
        }

        /// <summary>
        /// Writes avatar motion data to a CSV file on a worker thread.
        /// </summary>
        /// <param name="motionData">The avatar motion data to write.</param>
        /// <param name="dataPath">The output directory path relative to <see cref="Application.dataPath"/>.</param>
        /// <param name="fileName">The output file name, with or without the CSV extension.</param>
        /// <param name="token">The token used to cancel the operation.</param>
        /// <returns>The absolute path of the written CSV file.</returns>
        public async static ValueTask<string> WriteMotionDataAsync(AvatarMotionData motionData, string dataPath, string fileName, CancellationToken token)
        {
            return await Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                return WriteMotionData(motionData, dataPath, fileName);
            }, token);
        }

        /// <summary>
        /// Reads avatar motion data from a CSV file.
        /// </summary>
        /// <param name="bones">
        /// The avatar bones used to initialize and validate the motion tracks.
        /// Bone names must match those stored in the CSV file.
        /// </param>
        /// <param name="dataPath">The input directory path relative to <see cref="Application.dataPath"/>.</param>
        /// <param name="fileName">The input file name, with or without the CSV extension.</param>
        /// <returns>The avatar motion data read from the CSV file.</returns>
        public static AvatarMotionData ReadMotionData(IReadOnlyList<Transform> bones, string dataPath, string fileName)
        {
            // Check
            if (bones == null)
                throw new ArgumentNullException(nameof(bones));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("The file name must not be null or empty.", nameof(fileName));

            // Directory settings
            string directoryPath = GetDirectoryPath(dataPath);
            string normalizedFileName = Path.GetFileNameWithoutExtension(fileName) + CsvExtension;
            string filePath = Path.Combine(directoryPath, normalizedFileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("The avatar motion CSV file was not found.", filePath);

            using StreamReader reader = new(filePath, Utf8Encoding);

            // Read header
            if (reader.ReadLine() != MotionDataHeader)
                throw new InvalidDataException("The CSV header is invalid.");

            AvatarMotionData motionData = null;
            int expectedFrameIndex = 0;
            string firstBoneName = null;
            int lineNumber = 1;

            // Read motion data
            while (!reader.EndOfStream)
            {
                lineNumber++;
                string line = reader.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] values = line.Split(",");

                if (values.Length != ColumnCount)
                    throw new InvalidDataException($"Line {lineNumber} contains {values.Length} columns. " + $"Exactly {ColumnCount} columns are required.");

                int frameIndex = ParseInt(values[0], lineNumber, "frameIndex");
                string boneName = values[1];
                float time = ParseFloat(values[2], lineNumber, "time");

                Vector3 position = new(
                    ParseFloat(values[3], lineNumber, "posX"),
                    ParseFloat(values[4], lineNumber, "posY"),
                    ParseFloat(values[5], lineNumber, "posZ"));

                Quaternion rotation = new(
                    ParseFloat(values[6], lineNumber, "rotX"),
                    ParseFloat(values[7], lineNumber, "rotY"),
                    ParseFloat(values[8], lineNumber, "rotZ"),
                    ParseFloat(values[9], lineNumber, "rotW"));

                if (motionData == null)
                {
                    motionData = new AvatarMotionData(bones, 0);
                    firstBoneName = boneName;
                }

                if (!motionData.Positions.ContainsKey(boneName) || !motionData.Rotations.ContainsKey(boneName))
                    throw new InvalidDataException($"The motion data does not contain a track for bone '{boneName}'.");

                if (boneName == firstBoneName)
                {
                    if (frameIndex != expectedFrameIndex)
                        throw new InvalidDataException($"Unexpected frame index on line {lineNumber}. Expected {expectedFrameIndex}, but received {frameIndex}.");

                    motionData.Times.Add(time);
                    expectedFrameIndex++;
                }
                else if (frameIndex >= motionData.Times.Count)
                    throw new InvalidDataException($"Frame {frameIndex} for bone '{boneName}' appears before its time value was registered.");
                else if (!Mathf.Approximately(motionData.Times[frameIndex], time))
                    throw new InvalidDataException($"The time value for frame {frameIndex} is inconsistent on line {lineNumber}.");

                motionData.Positions[boneName].Add(position);
                motionData.Rotations[boneName].Add(rotation);
            }

            if (motionData == null)
                throw new InvalidDataException("The CSV file does not contain any motion data.");

            ValidateMotionData(motionData);
            return motionData;
        }

        /// <summary>
        /// Writes one avatar motion sample as a CSV row.
        /// </summary>
        /// <param name="writer">The destination text writer.</param>
        /// <param name="frameIndex">The zero-based frame index.</param>
        /// <param name="boneName">The recorded bone name.</param>
        /// <param name="time">The elapsed recording time in seconds.</param>
        /// <param name="position">The local bone position.</param>
        /// <param name="rotation">The local bone rotation.</param>
        private static void WriteMotionRow(StringBuilder row, TextWriter writer, int frameIndex, string boneName, float time, Vector3 position, Quaternion rotation)
        {
            row.Clear();
            row.Append(frameIndex);
            row.Append(',');
            row.Append(boneName);
            row.Append(',');
            AppendFloat(row, time);
            row.Append(',');
            AppendFloat(row, position.x);
            row.Append(',');
            AppendFloat(row, position.y);
            row.Append(',');
            AppendFloat(row, position.z);
            row.Append(',');
            AppendFloat(row, rotation.x);
            row.Append(',');
            AppendFloat(row, rotation.y);
            row.Append(',');
            AppendFloat(row, rotation.z);
            row.Append(',');
            AppendFloat(row, rotation.w);

            writer.WriteLine(row);
        }

        /// <summary>
        /// Validates that all motion tracks contain the same number of frames.
        /// </summary>
        /// <param name="motionData">The motion data to validate.</param>
        private static void ValidateMotionData(AvatarMotionData motionData)
        {
            int frameCount = motionData.FrameCount;

            if (motionData.Positions.Count != motionData.Rotations.Count)
                throw new InvalidDataException("The position and rotation track counts do not match.");

            foreach (KeyValuePair<string, List<Vector3>> positionEntry in motionData.Positions)
            {
                string boneName = positionEntry.Key;

                if (!motionData.Rotations.TryGetValue(boneName, out List<Quaternion> rotations))
                    throw new InvalidDataException($"The rotation track for bone '{boneName}' is missing.");

                if (positionEntry.Value.Count != frameCount)
                {
                    throw new InvalidDataException(
                        $"The position track for bone '{boneName}' contains " +
                        $"{positionEntry.Value.Count} frames, but " +
                        $"{frameCount} were expected.");
                }

                if (rotations.Count != frameCount)
                {
                    throw new InvalidDataException(
                        $"The rotation track for bone '{boneName}' contains " +
                        $"{rotations.Count} frames, but " +
                        $"{frameCount} were expected.");
                }
            }
        }

        /// <summary>
        /// Returns an absolute directory path under
        /// <see cref="Application.dataPath"/>.
        /// </summary>
        /// <param name="dataPath">The directory path relative to <see cref="Application.dataPath"/>.</param>
        /// <returns>The absolute directory path.</returns>
        private static string GetDirectoryPath(string dataPath)
        {
            string relativePath = string.IsNullOrWhiteSpace(dataPath) ? string.Empty : dataPath.Trim().TrimStart('/', '\\');
            return Path.Combine(Application.dataPath, relativePath);
        }

        /// <summary>
        /// Appends a floating-point value using culture-independent formatting.
        /// </summary>
        /// <param name="builder">The destination string builder.</param>
        /// <param name="value">The value to append.</param>
        private static void AppendFloat(StringBuilder builder, float value)
        {
            builder.Append(value.ToString("R", InvariantCulture));
        }

        /// <summary>
        /// Parses a floating-point CSV value.
        /// </summary>
        /// <param name="value">The text value to parse.</param>
        /// <param name="lineNumber">The source CSV line number.</param>
        /// <param name="columnName">The source CSV column name.</param>
        /// <returns>The parsed floating-point value.</returns>
        private static float ParseFloat(string value, int lineNumber, string columnName)
        {
            if (float.TryParse(value, NumberStyles.Float, InvariantCulture, out float result))
                return result;

            throw new InvalidDataException($"The value '{value}' in column '{columnName}' on line {lineNumber} is not a valid floating-point number.");
        }

        /// <summary>
        /// Parses an integer CSV value.
        /// </summary>
        /// <param name="value">The text value to parse.</param>
        /// <param name="lineNumber">The source CSV line number.</param>
        /// <param name="columnName">The source CSV column name.</param>
        /// <returns>The parsed integer value.</returns>
        /// <exception cref="InvalidDataException">
        /// Thrown when the value is not a valid integer.
        /// </exception>
        private static int ParseInt(string value, int lineNumber, string columnName)
        {
            if (int.TryParse(value, NumberStyles.Integer, InvariantCulture, out int result))
                return result;

            throw new InvalidDataException($"The value '{value}' in column '{columnName}' on line {lineNumber} is not a valid integer.");
        }
    }
}