﻿using System;
using Sharpmake;

namespace SharpmakeGen
{
    namespace FunctionalTests
    {
        public abstract class FunctionalTestProject : Common.SharpmakeBaseProject
        {
            public FunctionalTestProject()
                : base(excludeSharpmakeFiles: false, generateXmlDoc: false)
            {
                // same a samples, tests are special, the class is here instead of in the subfolder
                SourceRootPath = @"[project.SharpmakeCsPath]\[project.Name]";
                AddTargets(Common.GetDefaultTargets());
            }

            public override void ConfigureAll(Configuration conf, Target target)
            {
                base.ConfigureAll(conf, target);

                conf.SolutionFolder = "FunctionalTests";

                conf.AddPrivateDependency<SharpmakeProject>(target);
                conf.AddPrivateDependency<SharpmakeApplicationProject>(target);
                conf.AddPrivateDependency<SharpmakeGeneratorsProject>(target);
                conf.AddPrivateDependency<Platforms.CommonPlatformsProject>(target);
            }
        }

        [Generate]
        public class FastBuildFunctionalTest : FunctionalTestProject
        {
            public FastBuildFunctionalTest()
            {
                Name = "FastBuildFunctionalTest";
            }
        }
    }
}
