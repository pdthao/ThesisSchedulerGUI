using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ThesisSchedulerGUI
{
    public static class FuturePlanningSolverService
    {
        public static FuturePlanningResult Solve(
            FuturePlanningInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // =====================================================
            // 1. Define required paths
            // =====================================================

            string applicationFolder =
                Application.StartupPath;

            string scriptPath =
                Path.Combine(
                    applicationFolder,
                    "future_graduation_solver.py");

            string dataFolder =
                Path.Combine(
                    applicationFolder,
                    "Future Scheduler Data");

            string inputFilePath =
                Path.Combine(
                    dataFolder,
                    "input.json");

            string outputFilePath =
                Path.Combine(
                    dataFolder,
                    "output.json");

            // Create the data folder automatically if it does not exist.
            Directory.CreateDirectory(dataFolder);

            // =====================================================
            // 2. Check the Python solver file
            // =====================================================

            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException(
                    "The future graduation Python solver was not found.\n\n" +
                    "Expected location:\n" +
                    scriptPath,
                    scriptPath);
            }

            // =====================================================
            // 3. Convert and save C# input as JSON
            // =====================================================

            string inputJson =
                JsonConvert.SerializeObject(
                    input,
                    Formatting.Indented);

            File.WriteAllText(
                inputFilePath,
                inputJson,
                new UTF8Encoding(false));

            // =====================================================
            // 4. Configure the Python process
            // =====================================================

            ProcessStartInfo startInfo =
                new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = "\"" + scriptPath + "\"",

                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,

                    CreateNoWindow = true,

                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;

                // =================================================
                // 5. Start Python
                // =================================================

                try
                {
                    if (!process.Start())
                    {
                        throw new InvalidOperationException(
                            "The Python process could not be started.");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        "Failed to start Python.\n\n" +
                        "Please confirm that Python is installed and " +
                        "the command 'python' works in Command Prompt.",
                        ex);
                }

                // =================================================
                // 6. Send input JSON to Python
                // =================================================

                process.StandardInput.Write(inputJson);
                process.StandardInput.Close();

                // =================================================
                // 7. Read Python output
                // =================================================

                string rawOutput =
                    process.StandardOutput.ReadToEnd();

                string errorText =
                    process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (string.IsNullOrWhiteSpace(rawOutput))
                {
                    throw new InvalidOperationException(
                        "Python returned no output.\n\n" +
                        "Python error output:\n" +
                        errorText);
                }

                // =================================================
                // 8. Extract only the JSON object
                // =================================================
                // Gurobi may print license information or other text
                // before the JSON result.

                int jsonStartIndex =
                    rawOutput.IndexOf('{');

                int jsonEndIndex =
                    rawOutput.LastIndexOf('}');

                if (jsonStartIndex < 0 ||
                    jsonEndIndex < jsonStartIndex)
                {
                    throw new InvalidOperationException(
                        "Python did not return a valid JSON object.\n\n" +
                        "Python output:\n" +
                        rawOutput +
                        "\n\nPython error output:\n" +
                        errorText);
                }

                string outputJson =
                    rawOutput.Substring(
                        jsonStartIndex,
                        jsonEndIndex - jsonStartIndex + 1);

                // Format the output JSON before saving it.
                try
                {
                    object parsedJson =
                        JsonConvert.DeserializeObject(outputJson);

                    outputJson =
                        JsonConvert.SerializeObject(
                            parsedJson,
                            Formatting.Indented);
                }
                catch (JsonException)
                {
                    // Keep the extracted JSON unchanged.
                    // The next deserialization step will report details
                    // if the JSON is invalid.
                }

                // Save only the clean Python JSON result.
                File.WriteAllText(
                    outputFilePath,
                    outputJson,
                    new UTF8Encoding(false));

                // =================================================
                // 9. Convert JSON into FuturePlanningResult
                // =================================================

                FuturePlanningResult result;

                try
                {
                    result =
                        JsonConvert.DeserializeObject
                            <FuturePlanningResult>(
                                outputJson);
                }
                catch (JsonException ex)
                {
                    throw new InvalidOperationException(
                        "The Python output could not be converted into " +
                        "FuturePlanningResult.\n\n" +
                        "Extracted JSON:\n" +
                        outputJson +
                        "\n\nPython error output:\n" +
                        errorText,
                        ex);
                }

                if (result == null)
                {
                    throw new InvalidOperationException(
                        "The Python solver returned an empty result.");
                }

                // =================================================
                // 10. Check the Python exit code
                // =================================================

                if (process.ExitCode != 0)
                {
                    string solverMessage =
                        string.IsNullOrWhiteSpace(result.message)
                            ? "Unknown Python solver error."
                            : result.message;

                    throw new InvalidOperationException(
                        "The future graduation solver failed.\n\n" +
                        "Status: " + result.status + "\n" +
                        "Message: " + solverMessage + "\n\n" +
                        "Python error output:\n" +
                        errorText);
                }

                return result;
            }
        }
    }
}