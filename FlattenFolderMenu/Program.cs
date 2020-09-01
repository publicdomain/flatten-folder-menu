﻿// <copyright file="Program.cs" company="PublicDomain.com">
//     CC0 1.0 Universal (CC0 1.0) - Public Domain Dedication
//     https://creativecommons.org/publicdomain/zero/1.0/legalcode
// </copyright>
// <auto-generated />

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Runtime.CompilerServices;
using System.Linq;

namespace FlattenFolderMenu
{
    /// <summary>
    /// Class with program entry point.
    /// </summary>
    internal sealed class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check for args
            if (args.Length > 0)
            {
                // Try block with error message
                try
                {
                    // Set directory path
                    string directoryPath = args[0];

                    /* Flatten Folder */

                    // Collect subdirectories
                    string[] subdirectoriesArray = Directory.GetDirectories(directoryPath);

                    // Check there's something to work with
                    if (subdirectoriesArray.Length == 0)
                    {
                        // Advise user
                        MessageBox.Show("Folder is already flattened!", "Flattened", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Halt flow
                        return;
                    }

                    // Declare file list
                    List<string> fileList = new List<string>();

                    // Collect files in subdirectories
                    foreach (var subdirectory in subdirectoriesArray)
                    {
                        // Add to file list
                        fileList.AddRange(Directory.GetFiles(subdirectory, "*", SearchOption.AllDirectories));
                    }

                    // Set ies or Y
                    string iesOrY = subdirectoriesArray.Length > 1 ? "ies" : "y";

                    // Check there's something to work with
                    if (fileList.Count == 0)
                    {
                        // Set message
                        string message = "No files to flatten!";

                        // Check for subdirectories
                        if (subdirectoriesArray.Length > 0)
                        {
                            // Ask user
                            if (MessageBox.Show($"{message}{Environment.NewLine}Delete {subdirectoriesArray.Length} subdirector{iesOrY}?", "Empty subdirectories", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                // Remove subdirectories and advice user
                                goto bottomLabel;
                            }
                            else
                            {
                                // Halt flow
                                return;
                            }
                        }
                        else
                        {
                            // Advise user
                            MessageBox.Show($"{message}", "Flattened", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Halt flow
                            return;
                        }
                    }

                    /* TODO Flatten procedure [File renaming can be refined] */

                    // TODO Set epoch [Can be set after checking file exists. Set here to indicate multiple files from the same flattening batch]
                    int epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

                    // Process files in list
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        // Set file
                        var file = fileList[i];

                        // Set target file
                        var targetFile = Path.Combine(directoryPath, Path.GetFileName(file));

                        // Check file exists
                        if (File.Exists(targetFile))
                        {
                            // Set target file with epoch + number
                            targetFile = Path.Combine(directoryPath, $"{Path.GetFileNameWithoutExtension(file)}-{epoch}-{i}{Path.GetExtension(file)}");
                        }

                        // Move current one
                        File.Move(file, targetFile);
                    }

                // Bottom label
                bottomLabel:

                    // Iterate subdirectories
                    foreach (var subdirectory in subdirectoriesArray)
                    {
                        // Remove subdirectory
                        new DirectoryInfo(subdirectory).Delete(true);
                    }

                    // Directory was flattened successfully. Advise user
                    MessageBox.Show($"\"{Path.GetDirectoryName(directoryPath)}\" has been flattened!{Environment.NewLine}(Processed: {fileList.Count} file{(fileList.Count > 0 ? "s" : string.Empty)}, {subdirectoriesArray.Length} subdirector{iesOrY})", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Error, advise user
                    MessageBox.Show($"Error while flattening folder. Message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Regular execution
                Application.Run(new MainForm());
            }
        }
    }
}
