// https://makolyte.com/auto-increment-build-numbers-in-visual-studio/


using System.Reflection;

[assembly: System.Reflection.AssemblyCompanyAttribute("Lab Patient Accounting")]
[assembly: System.Reflection.AssemblyCopyrightAttribute("Copyright 2020-2024")]
[assembly: System.Reflection.AssemblyDescriptionAttribute("...")]
#if RELEASE
[assembly: System.Reflection.AssemblyFileVersionAttribute("1.0.0.0")]
[assembly: System.Reflection.AssemblyInformationalVersionAttribute("1.0.0.0")]
[assembly: AssemblyVersion("1.0.0.0")]
#else
[assembly: System.Reflection.AssemblyFileVersionAttribute("1.0.0.0")]
[assembly: System.Reflection.AssemblyInformationalVersionAttribute("1.0.0.0")]
[assembly: AssemblyVersion("1.0.0.0")]
#endif

