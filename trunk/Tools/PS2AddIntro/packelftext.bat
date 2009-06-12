set CYGWIN=E:/OldPC/cygwin/bin
set PS2DEV=E:/OldPC/PS2DEV/ps2dev
set PS2SDK=E:/OldPC/PS2DEV/ps2dev/ps2sdk
set PS2SDKSRC=E:/OldPC/PS2DEV/SRC/ps2sdk
set PATH=%PS2DEV%/bin;%PS2DEV%/ee/bin;%PS2DEV%/iop/bin;%PS2DEV%/dvp/bin;%PS2SDK%/bin;%CYGWIN%;%PATH%

E:\CNTranslation\PS2\Common\AddIntro\uclpack c slpm_text slpm.ucl

bin2s slpm.ucl slpm.s elftext_ucl_data .copy2ram_data




