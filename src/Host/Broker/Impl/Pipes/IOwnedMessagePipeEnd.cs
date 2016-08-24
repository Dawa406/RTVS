﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace Microsoft.R.Host.Broker.Pipes {
    public interface IOwnedMessagePipeEnd : IMessagePipeEnd, IDisposable {
    }
}
