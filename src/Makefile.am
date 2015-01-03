tkddir=$(pkglibdir)
tkd_SCRIPTS = tkd.exe
EXTRA_DIST = $(tkd_sources)
CLEANFILES = tkd.exe

tkd_sources = Main.cs MainWindow.cs AssemblyInfo.cs Test.cs ReadTSV.cs Dialog.cs YesNoDialog.cs EndOfTestDialog.cs loadsaveconfig.cs

tkd_build_sources = $(addprefix $(srcdir)/, $(tkd_sources))

tkd.exe: $(tkd_build_sources)
	$(MCS) $(GTKSHARP_LIBS) -out:$@ $(tkd_build_sources) 

