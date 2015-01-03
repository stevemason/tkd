SUBDIRS = src data desktop
bin_SCRIPTS = tkd
EXTRA_DIST = script.in

REWRITE = \
        sed -e "s|\@prefix\@|$(prefix)|g"               \
            -e "s|\@pkglibdir\@|$(pkglibdir)|g"         \
            -e "s|\@bindir\@|$(bindir)|g"		\
	    -e "s|\@MONO\@|$(MONO)|g" 

$(bin_SCRIPTS): $(srcdir)/script.in Makefile
	$(REWRITE) $(srcdir)/script.in > $@.tmp
	mv $@.tmp $@

clean-local:
	-rm -f tkd

uninstall-local:
	-rmdir $(pkglibdir)
