﻿// Copyright 2020-2021 Andreas Atteneder
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using UnityEngine;

namespace Draco.Tests {

    public class UseDracoTestFileCaseAttribute : UnityEngine.TestTools.UnityTestAttribute, ITestBuilder {

        string[] m_sampleSet = null;

        NUnitTestCaseBuilder _builder = new NUnitTestCaseBuilder();

        public UseDracoTestFileCaseAttribute(string[] sampleSetPath) {
            m_sampleSet = sampleSetPath;
        }

        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, Test suite) {
            List<TestMethod> results = new List<TestMethod>();
            var nameCounts = new Dictionary<string, int>();

            if (m_sampleSet == null) {
                throw new Exception("SampleSet not set");
            }

            try {
                foreach (var testCase in m_sampleSet) {
                    var data = new TestCaseData(new object[] { testCase });

                    var uri = new Uri(testCase);

                    string origName = System.IO.Path.GetFileName(uri.LocalPath);
                    string name;
                    if (nameCounts.TryGetValue(origName, out int count)) {
                        name = string.Format("{0}-{1}", origName, count);
                        nameCounts[origName] = count + 1;
                    }
                    else {
                        name = origName;
                        nameCounts[origName] = 1;
                    }

                    data.SetName(name);
                    data.ExpectedResult = new UnityEngine.Object();
                    data.HasExpectedResult = true;

                    var test = this._builder.BuildTestMethod(method, suite, data);
                    if (test.parms != null)
                        test.parms.HasExpectedResult = false;

                    test.Name = name;

                    results.Add(test);
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Failed to generate glTF testcases!");
                Debug.LogException(ex);
                throw;
            }

            Console.WriteLine("Generated {0} glTF test cases.", results.Count);
            return results;
        }
    }
}
