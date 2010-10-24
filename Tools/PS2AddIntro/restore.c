extern char elftext_ucl_data[];
extern char text_asm_end[];
extern char _start_bss[];
extern char _end_bss[];

void PatchExe(unsigned int* args) __attribute__ ((section (".copy2ram_text")));
extern void UnpackText(void *ram_ucl_data, void *unpack_dst, unsigned int entry) __attribute__ ((section (".copy2ram_text")));

void RestoreProcess()
{
	memset(_start_bss,0,(_end_bss-_start_bss)+HEAPSIZE);
	memcpy(_start_bss,elftext_ucl_data,text_asm_end-elftext_ucl_data);
	UnpackText(_start_bss,(void*)TEXTSTARTADR,ELFENTRY);
}

void PatchExe(unsigned int* args)
{
	return;
}
