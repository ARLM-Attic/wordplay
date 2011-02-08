#
# Variables
#

BUILD      = build
SRC        = src
LIBS       = libs

CS         = /usr/bin/gmcs -debug
RESGEN     = /usr/bin/resgen
RCOPY      = rsync --cvs-exclude

EXE        = $(BUILD)/mfgames-wordplay.exe
EXE_FILES  = $(shell find $(SRC) -name "*.cs")
EXE_RES    = $(SRC)/menu.xml

LOCALE_XML = $(wildcard src/locale/*.resx)
LOCALE_RES = $(LOCALE_XML:resx=resources)

#
# Primary Targets
#

all: init $(EXE) copy-themes

clean:

init:
	mkdir -p build

#
# Executable
#

$(EXE): $(EXE_FILES) $(EXE).config $(LOCALE_RES)
	$(CS) /out:$(EXE) /t:exe \
                -pkg:log4net \
                -pkg:mfgames-log4net-1 \
                -pkg:mfgames-utility-1 \
		-pkg:mfgames-gtkext-2 \
		-pkg:gtk-sharp-2.0 \
		-pkg:rsvg-sharp-2.0 \
		-r:$(LIBS)/NetSpell.SpellChecker \
		-r:$(LIBS)/C5 \
		-r:$(LIBS)/MfGames.Sprite \
                $(shell for i in $(EXE_RES) $(LOCALE_RES); do \
                        echo -resource:$(PWD)/$$i,$$(basename $$i); done) \
		$(shell for i in $(EXE_FILES); do \
                        echo $(PWD)/$$i; done)

$(EXE).config: $(SRC)/config.xml
	cp $(SRC)/config.xml $(EXE).config

%.resources: %.resx
	$(RESGEN) $*.resx

copy-themes:
	$(RCOPY) -Lra $(SRC)/themes/ build/themes/
	$(RCOPY) -ra $(SRC)/dicts/ build/dicts/
