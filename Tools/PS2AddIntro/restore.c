extern char elftext_ucl_data[];
extern char text_asm_end[];
extern char _end_bss[];
extern int _heap_size;
void UnpackText(void *ram_ucl_data, void *unpack_dst, unsigned int entry);

void RestoreProcess()
{
	memcpy(_end_bss+HEAPSIZE,elftext_ucl_data,text_asm_end-elftext_ucl_data);
	UnpackText(_end_bss+HEAPSIZE,(void*)0x100000,0x100000);
}

