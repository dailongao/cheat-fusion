include $(PS2SDK)/Defs.make
INTRODIR = E:/CNTranslation/PS2/Common/AddIntro/

UCL_ELFTEXTFILE = slpm
USR_RESTOREFILE = restore
USR_STARTADR = 0x100000
USR_STARTBSS = 0x400000
USR_HEAPSIZE = 0x10000



EE_CPPFLAGS = -G0 -I$(PS2SDK)/ee/include -I$(PS2SDK)/common/include -I . -Os -Wall -DHEAPSIZE=$(USR_HEAPSIZE)
EE_LDFLAGS = -T $(INTRODIR)linkfile -Wl,--defsym,HEAPSIZE=$(USR_HEAPSIZE) -L$(PS2SDK)/ee/lib $(EE_LIBS) -lc -lkernel -lsyscall

EE_INCS += -IE:/OldPC/PS2DEV/Project/SCElib
EE_LDFLAGS += -LE:/OldPC/PS2DEV/Project/SCElib
EE_LIBS = -lSCEtim2 -lSCEgraph -ldraw -lgraph -lpacket -ldma -lgs -lpad


TARGETS = intro.elf

all: $(TARGETS)
	ee-strip --strip-all $(TARGETS)

intro.elf: $(INTRODIR)crt0.o $(USR_RESTOREFILE).o main.o $(UCL_ELFTEXTFILE).o $(INTRODIR)unpack.S
	$(EE_CC) -nostartfiles -Wl,--defsym,_start_address=$(USR_STARTADR) -o $(TARGETS) \
	$(INTRODIR)crt0.o $(USR_RESTOREFILE).o main.o $(UCL_ELFTEXTFILE).o $(INTRODIR)unpack.S \
	$(EE_LDFLAGS) -Wl,--defsym,_start_bss=$(USR_STARTBSS)

clean:
	rm -f *.o $(TARGETS)

rebuild: clean all

%.o : %.c
	$(EE_CC) $(EE_CFLAGS) $(EE_CPPFLAGS) $(EE_INCS) -c $< -o $@

%.o : %.s
	$(EE_AS) $(EE_ASFLAGS) $< -o $@


