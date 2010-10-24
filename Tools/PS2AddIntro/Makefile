include $(PS2SDK)/Defs.make
INTRODIR = E:/CNTranslation/PS2/Common/AddIntro/

UCL_ELFTEXTFILE = slps
USR_RESTOREFILE = restore

USR_STARTADR = 0x100000
USR_STARTBSS = 0x400000
USR_HEAPSIZE = 0x10000

ELF_ENTRY = 0x100008





EE_CPPFLAGS = -G0 -I$(PS2SDK)/ee/include -I$(PS2SDK)/common/include -I . -O2 -Wall \
	-DHEAPSIZE=$(USR_HEAPSIZE) -DTEXTSTARTADR=$(USR_STARTADR) -DELFENTRY=$(ELF_ENTRY)
EE_LDFLAGS = -T $(INTRODIR)linkfile -Wl,--defsym,HEAPSIZE=$(USR_HEAPSIZE) \
		-L$(PS2SDK)/ee/lib $(EE_LIBS) -lc -lkernel -lsyscall

EE_INCS += -IE:/OldPC/PS2DEV/Project/SCElib -I$(INTRODIR)
EE_LDFLAGS += -LE:/OldPC/PS2DEV/Project/SCElib
EE_LIBS = -lSCEtim2 -lSCEgraph -ldraw -lgraph -lpacket -ldma -lgs -lpad


TARGETS = intro.elf

all: $(TARGETS)
	ee-strip --strip-all $(TARGETS)

intro.elf: $(INTRODIR)crt0.o main.o $(UCL_ELFTEXTFILE).o $(USR_RESTOREFILE).o $(INTRODIR)unpack.S
	$(EE_CC) -nostartfiles -Wl,--defsym,_start_address=$(USR_STARTADR) -o $(TARGETS) \
	$(INTRODIR)crt0.o main.o $(UCL_ELFTEXTFILE).o  $(USR_RESTOREFILE).o $(INTRODIR)unpack.S \
	$(EE_LDFLAGS) -Wl,--defsym,_start_bss=$(USR_STARTBSS) -Wl,-Map,intro.map

clean:
	rm -f *.o $(TARGETS)

rebuild: clean all

%.o : %.c
	$(EE_CC) $(EE_CFLAGS) $(EE_CPPFLAGS) $(EE_INCS) -c $< -o $@

%.o : %.s
	$(EE_AS) $(EE_ASFLAGS) $< -o $@


