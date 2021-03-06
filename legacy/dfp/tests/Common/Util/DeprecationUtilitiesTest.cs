﻿// Copyright 2013, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// Author: api.anash@gmail.com (Anash P. Oommen)

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Google.Api.Ads.Common.Util;
using System.Diagnostics;
using System.IO;

namespace Google.Api.Ads.Common.Tests.Util {
  /// <summary>
  /// Tests for DeprecationUtilities class.
  /// </summary>
  [TestFixture]
  public class DeprecationUtilitiesTest {
    private const string DEPRECATION_MESSAGE = "This is a deprecation message.";

#pragma warning disable 414
    [Obsolete(DEPRECATION_MESSAGE)]
    private string deprecatedField = "";

    private string nonDeprecatedField = "";
#pragma warning restore 414

    private MemberInfo deprecatedMemberInfo;
    private MemberInfo nonDeprecatedMemberInfo;
    private MemoryStream memStream;

    /// <summary>
    /// Inits this instance.
    /// </summary>
    [SetUp]
    public void Init() {
      memStream = new MemoryStream();
      TextWriterTraceListener textListener = new TextWriterTraceListener(memStream);
      Trace.Listeners.Add(textListener);
      deprecatedMemberInfo = this.GetType().GetField("deprecatedField",
          BindingFlags.NonPublic | BindingFlags.Instance);
      nonDeprecatedMemberInfo = this.GetType().GetField("nonDeprecatedField",
          BindingFlags.NonPublic | BindingFlags.Instance);
    }

    /// <summary>
    /// Tests if deprecation methods are displayed for deprecated fields.
    /// </summary>
    [Test]
    public void TestShowsDeprecationForDeprecatedField() {
      DeprecationUtilities.ShowDeprecationMessage(deprecatedMemberInfo);
      String traceContents = Encoding.UTF8.GetString(memStream.ToArray());
      if (!traceContents.Contains(DEPRECATION_MESSAGE)) {
        Assert.Fail("Deprecation message is missing in trace logs.");
      }
    }

    /// <summary>
    /// Tests if deprecation methods not displayed for non-deprecated fields.
    /// </summary>
    [Test]
    public void TestDoesNotShowDeprecationForNonDeprecatedField() {
      DeprecationUtilities.ShowDeprecationMessage(nonDeprecatedMemberInfo);
      String traceContents = Encoding.UTF8.GetString(memStream.ToArray());
      if (traceContents.Contains(DEPRECATION_MESSAGE)) {
        Assert.Fail("Deprecation message is present in trace logs.");
      }
    }
  }
}
