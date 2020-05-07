﻿// Copyright (c) 2017 Ubisoft Entertainment
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
namespace Sharpmake.Generators.Generic
{
    public partial class Makefile
    {
        public static class Template
        {
            public static class Solution
            {
                public static string Header =
@"# Generated by Sharpmake -- Do not edit !
# Type ""make help"" for usage help

ifndef config
  config=[defaultConfig]
endif
export config

";
                public static string ProjectsVariableBegin = "PROJECTS := \\\n";

                public static string ProjectsVariableElement = "\t[projectName] \\\n";

                public static string ProjectsVariableEnd = "\n";

                public static string PhonyTargets = ".PHONY: all clean help $(PROJECTS)\n\n";

                public static string AllRule = "all: $(PROJECTS)\n\n";

                public static string ProjectRuleBegin = "[projectName]: ";

                public static string ProjectRuleDependency = "[dependencyName] ";

                public static string ProjectRuleEnd =
                    "\n\t@echo \" ==== Building [projectName] ($(config)) ====\"" +
                    "\n\t@${MAKE} --no-print-directory -C [projectFileDirectory] -f [projectFileName]\n\n";

                public static string CleanRuleBegin = "clean:\n";

                public static string CleanRuleProject = "\t@${MAKE} --no-print-directory -C [projectFileDirectory] -f [projectFileName] clean\n";

                public static string CleanRuleEnd = "\n";

                public static string HelpRuleBegin =
                    "help:\n" +
                    "\t@echo \"Usage: make [config = name] [target]\"\n" +
                    "\t@echo \"\"\n" +
                    "\t@echo \"CONFIGURATIONS:\"\n";

                public static string HelpRuleConfiguration = "\t@echo \"   [optimization]\"\n";

                public static string HelpRuleTargetsBegin =
                    "\t@echo \"\"\n" +
                    "\t@echo \"TARGETS:\"\n" +
                    "\t@echo \"   all (default)\"\n" +
                    "\t@echo \"   clean\"\n";

                public static string HelpRuleTarget = "\t@echo \"   [projectName]\"\n";

                public static string HelpRuleEnd = "\n";
            }

            public static class Project
            {
                public static string Header =
@"# Generated by Sharpmake -- Do not edit !
ifndef config
  config=debug
endif

ifndef verbose
  SILENT = @
endif

";

                public static string ProjectConfigurationVariables =
@"ifeq ($(config),[name])
  CXX        = [options.CompilerToUse]
  AR         = ar
  OBJDIR     = [options.IntermediateDirectory]
  TARGETDIR  = [options.OutputDirectory]
  TARGET     = $(TARGETDIR)/[options.OutputFile]
  DEFINES   += [options.Defines]
  INCLUDES  += [options.Includes]
  CPPFLAGS  += -MMD -MP $(DEFINES) $(INCLUDES)
  CFLAGS    += $(CPPFLAGS) [options.CFLAGS]
  CXXFLAGS  += $(CFLAGS) [options.CXXFLAGS]
  LDFLAGS   += [options.LibraryPaths] [options.AdditionalLinkerOptions]
  LDLIBS    += [options.LibsStartGroup][options.DependenciesLibraryFiles] [options.LibraryFiles][options.LibsEndGroup]
  RESFLAGS  += $(DEFINES) $(INCLUDES)
  LDDEPS    += [options.LDDEPS]
  LINKCMD    = [options.LinkCommand]
  define PREBUILDCMDS
  endef
  define PRELINKCMDS
  endef
  define POSTBUILDCMDS
  endef
  define POSTFILECMDS
  endef
endif

";
                public static string LinkCommandLib = "$(AR) -rcs $(TARGET) $(OBJECTS)";

                public static string LinkCommandExe = "$(CXX) -o $(TARGET) $(OBJECTS) $(LDFLAGS) $(RESOURCES) $(LDLIBS)";

                public static string ObjectsVariableBegin = "ifeq ($(config),[name])\n";

                public static string ObjectsVariableElement = "  [excludeChar]OBJECTS += $(OBJDIR)/[objectFile]\n";

                public static string ObjectsVariableEnd = "endif\n\n";

                public static string ProjectRulesGeneral =
@"RESOURCES := \

SHELLTYPE := msdos
ifeq (,$(ComSpec)$(COMSPEC))
  SHELLTYPE := posix
endif
ifeq (/bin,$(findstring /bin,$(SHELL)))
  SHELLTYPE := posix
endif

.PHONY: clean prebuild prelink

all: $(TARGETDIR) $(OBJDIR) prebuild prelink $(TARGET)
	@:

$(TARGET): $(GCH) $(OBJECTS) $(LDDEPS) $(RESOURCES) | $(TARGETDIR)
	@echo Linking [projectName]
	$(SILENT) $(LINKCMD)
	$(POSTBUILDCMDS)

$(TARGETDIR):
	@echo Creating $(TARGETDIR)
ifeq (posix,$(SHELLTYPE))
	$(SILENT) mkdir -p $(TARGETDIR)
else
	$(SILENT) if not exist $(subst /,\\,$(TARGETDIR)) mkdir $(subst /,\\,$(TARGETDIR))
endif

ifneq ($(OBJDIR),$(TARGETDIR))
$(OBJDIR):
	@echo Creating $(OBJDIR)
ifeq (posix,$(SHELLTYPE))
	$(SILENT) mkdir -p $(OBJDIR)
else
	$(SILENT) if not exist $(subst /,\\,$(OBJDIR)) mkdir $(subst /,\\,$(OBJDIR))
endif
endif

clean:
	@echo Cleaning [projectName]
ifeq (posix,$(SHELLTYPE))
	$(SILENT) rm -f  $(TARGET)
	$(SILENT) rm -rf $(OBJDIR)
else
	$(SILENT) if exist $(subst /,\\,$(TARGET)) del $(subst /,\\,$(TARGET))
	$(SILENT) if exist $(subst /,\\,$(OBJDIR)) rmdir /s /q $(subst /,\\,$(OBJDIR))
endif

prebuild:
	$(PREBUILDCMDS)

prelink:
	$(PRELINKCMDS)

ifneq (,$(PCH))
$(GCH): $(PCH)
	@echo $(notdir $<)
	-$(SILENT) cp $< $(OBJDIR)
	$(SILENT) $(CXX) $(CXXFLAGS) -o ""$@"" -c ""$<""
	$(SILENT) $(POSTFILECMDS)
endif

";

                public static readonly string ObjectRuleCxx =
@"$(OBJDIR)/[objectFile]: [sourceFile] | $(OBJDIR)
	@echo $(notdir $<)
	$(SILENT) $(CXX) $(CXXFLAGS) -o ""$@"" -c ""$<""
	$(SILENT) $(POSTFILECMDS)

";

                public static readonly string ObjectRuleC =
@"$(OBJDIR)/[objectFile]: [sourceFile] | $(OBJDIR)
	@echo $(notdir $<)
	$(SILENT) $(CXX)  $(CFLAGS) -x c -o ""$@"" -c ""$<""
	$(SILENT) $(POSTFILECMDS)

";

                public static string Footer = "-include $(OBJECTS:%.o=%.d)\n";
            }
        }
    }
}
