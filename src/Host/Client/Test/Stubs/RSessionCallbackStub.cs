﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Common.Core.Shell;

namespace Microsoft.R.Host.Client.Test.Stubs {
    public class RSessionCallbackStub : IRSessionCallback {
        public IList<string> ShowErrorMessageCalls { get; } = new List<string>();
        public IList<Tuple<string, MessageButtons>> ShowMessageCalls { get; } = new List<Tuple<string, MessageButtons>>();
        public IList<string> HelpUrlCalls { get; } = new List<string>();
        public IList<Tuple<PlotMessage, CancellationToken>> PlotCalls { get; } = new List<Tuple<PlotMessage, CancellationToken>>();
        public IList<CancellationToken> LocatorCalls { get; } = new List<CancellationToken>();
        public IList<Tuple<Guid, CancellationToken>> PlotDeviceCreateCalls { get; } = new List<Tuple<Guid, CancellationToken>>();
        public IList<Tuple<Guid, CancellationToken>> PlotDeviceDestroyCalls { get; } = new List<Tuple<Guid, CancellationToken>>();
        public IList<Tuple<string, int, CancellationToken>> ReadUserInputCalls { get; } = new List<Tuple<string, int, CancellationToken>>();
        public IList<string> CranUrlFromNameCalls { get; } = new List<string>();
        public IList<Tuple<string, string>> ViewObjectCalls { get; } = new List<Tuple<string, string>>();
        public IList<int> ViewLibraryCalls { get; } = new List<int>();
        public IList<Tuple<string, string, bool>> ShowFileCalls { get; } = new List<Tuple<string, string, bool>>();
        public IList<Tuple<string, byte[]>> SaveFileCalls { get; } = new List<Tuple<string, byte[]>>();

        public Func<string, MessageButtons, Task<MessageButtons>> ShowMessageCallsHandler { get; set; } = (m, b) => Task.FromResult(MessageButtons.OK);
        public Func<string, int, CancellationToken, Task<string>> ReadUserInputHandler { get; set; } = (m, l, ct) => Task.FromResult("\n");
        public Func<Guid, CancellationToken, Task<LocatorResult>> LocatorHandler { get; set; } = (deviceId, ct) => Task.FromResult(LocatorResult.CreateNotClicked());
        public Func<Guid, CancellationToken, Task<PlotDeviceProperties>> PlotDeviceCreateHandler { get; set; } = (deviceId, ct) => Task.FromResult(PlotDeviceProperties.CreateDefault());
        public Func<Guid, CancellationToken, Task> PlotDeviceDestroyHandler { get; set; } = (deviceId, ct) => Task.CompletedTask;

        public Func<string, string> CranUrlFromNameHandler { get; set; } = s => "https://cran.rstudio.com";
        public Func<string, string, Task> ViewObjectHandler { get; set; } = (x, t) => Task.CompletedTask;
        public Action ViewLibraryHandler { get; set; } = () => { };
        public Func<string, string, bool, Task> ShowFileHandler { get; set; } = (f, t, d) => Task.CompletedTask;
        public Func<string, byte[], Task<string>> SaveFileHandler { get; set; } = (f, d) => Task.FromResult(string.Empty);

        public Task ShowErrorMessage(string message) {
            ShowErrorMessageCalls.Add(message);
            return Task.CompletedTask;
        }

        public Task<MessageButtons> ShowMessage(string message, MessageButtons buttons) {
            ShowMessageCalls.Add(new Tuple<string, MessageButtons>(message, buttons));
            return ShowMessageCallsHandler != null ? ShowMessageCallsHandler(message, buttons) : Task.FromResult(default(MessageButtons));
        }

        public Task ShowHelp(string url) {
            HelpUrlCalls.Add(url);
            return Task.CompletedTask;
        }

        public Task Plot(PlotMessage plot, CancellationToken ct) {
            PlotCalls.Add(new Tuple<PlotMessage, CancellationToken>(plot, ct));
            return Task.CompletedTask;
        }

        public Task<LocatorResult> Locator(Guid deviceId, CancellationToken ct) {
            LocatorCalls.Add(ct);
            return LocatorHandler != null ? LocatorHandler(deviceId, ct) : Task.FromResult(LocatorResult.CreateNotClicked());
        }

        public Task<PlotDeviceProperties> PlotDeviceCreate(Guid deviceId, CancellationToken ct) {
            PlotDeviceCreateCalls.Add(new Tuple<Guid, CancellationToken>(deviceId, ct));
            return PlotDeviceCreateHandler != null ? PlotDeviceCreateHandler(deviceId, ct) : Task.FromResult(PlotDeviceProperties.CreateDefault());
        }

        public Task PlotDeviceDestroy(Guid deviceId, CancellationToken ct) {
            PlotDeviceDestroyCalls.Add(new Tuple<Guid, CancellationToken>(deviceId, ct));
            return PlotDeviceDestroyHandler != null ? PlotDeviceDestroyHandler(deviceId, ct) : Task.CompletedTask;
        }

        public Task<string> ReadUserInput(string prompt, int maximumLength, CancellationToken ct) {
            ReadUserInputCalls.Add(new Tuple<string, int, CancellationToken>(prompt, maximumLength, ct));
            return ReadUserInputHandler != null ? ReadUserInputHandler(prompt, maximumLength, ct) : Task.FromResult(string.Empty);
        }

        public string CranUrlFromName(string name) {
            CranUrlFromNameCalls.Add(name);
            return CranUrlFromNameHandler != null ? CranUrlFromNameHandler(name) : string.Empty;
        }

        public void ViewObject(string expression, string title) {
            ViewObjectCalls.Add(new Tuple<string, string>(expression, title));
            ViewObjectHandler?.Invoke(expression, title);
        }

        public Task ViewLibrary() {
            ViewLibraryCalls.Add(0);
            ViewLibraryHandler?.Invoke();
            return Task.CompletedTask;
        }

        public Task ViewFile(string fileName, string tabName, bool deleteFile) {
            ShowFileCalls.Add(new Tuple<string, string, bool>(fileName, tabName, deleteFile));
            ShowFileHandler?.Invoke(fileName, tabName, deleteFile);
            return Task.CompletedTask;
        }

        public Task<string> SaveFileAsync(string fileName, byte[] data) {
            SaveFileCalls.Add(new Tuple<string, byte[]>(fileName, data));
            SaveFileHandler?.Invoke(fileName, data);
            return Task.FromResult(string.Empty);
        }
    }
}
