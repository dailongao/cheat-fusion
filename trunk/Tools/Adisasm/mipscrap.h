extern char ITypeMnem[64][8];
extern char RTypeMnem[64][8];
extern char RITypeMnem[32][10];
extern char SysCoMnem[32][10];
extern char GTEMnem[32][12];
extern char CPURegStr[35][16];
extern char SysCoRegStr[32][16];
extern char GTERegStr[32][16];
extern char GTERegStr2[32][16];

#define DECODE_RS(d)     ((char)((d&0x3e00000)>>21))
#define DECODE_RT(d)     ((char)((d&0x1f0000)>>16))
#define DECODE_RD(d)     ((char)((d&0xf800)>>11))
#define DECODE_SA(d)     ((char)((d&0x7c0)>>6))
#define DECODE_IMMED(d)  ((signed short)(d&0x0000ffff))
#define DECODE_OFFSET(d,pc) (((signed short)(d&0x0000ffff)<<2)+pc+4)
#define DECODE_FUNC(d)		 ((char)(d&0x3f))
#define DECODE_INDEX(d,pc)  ((d&0x3ffffff)<<2|((pc+4)&0xf0000000))
#define DECODE_COPFUN(d) ((word)(d&0x3ffffc0)>>6) 
#define DECODE_OP(d)     ((char)(d>>26))			

#define I_JRRA 0x03e00008

// encoded in opcode field // IType

#define opSpecial 0
#define opRegImm  1

// no reg is changed, except PC and ra (only by JAl)
#define opJ       2
#define opJAl     3
#define opBeq     4
#define opBne     5  
#define opBlez    6 // no rt
#define opBgtz    7 // no rt

/////////////////////////////////////////////
// in general: rt = rs o immed 
// where o represents some kind of operator

#define opAddI    8
#define opAddIU   9
#define opSltI    10
#define opSltIU   11
#define opAndI    12
#define opOrI     13
#define opXorI    14
#define opLUI     15 // no rs

#define opCop0    16
#define opCop1    17
#define opCop2    18
#define opCop3    19

// rt is put in/got from mem location specified by rs+immed
#define opLB      32
#define opLH      33
#define opLWL     34
#define opLW      35
#define opLBU     36
#define opLHU     37
#define opLWR     38
#define opSB      40
#define opSH      41
#define opSWL     42
#define opSW      43
#define opSWR     46
#define opLWC1    49
#define opLWC2    50
#define opLWC3    51
#define opSWC1    57
#define opSWC2    58
#define opSWC3    59

// these defines are for indexing CopMnem (encoded in rs field)
// they are the same for all four coprocessors

#define copMfC			 0
#define copCfC    2
#define copMtC				4
#define copCtC				6
#define cop0RestoreFromException 16

// these defines are for indexing the RITypeMnem (encoded in rt field) IType

/////////////////////////////////////////////
// in general: pc = immed (if rs o rt) or remains unchanged otherwise
// where o represents some kind of operator that results in a bool

#define riBltz 0
#define riBgez 1
#define riBltzAl 16
#define riBgezAl 17

// these defines are for indexing the RTypeMnem (encoded in func field) RType


/////////////////////////////////////////////
// in general: rd = rt o rs(or sa)
//                = HI/LO
//                = remains unchanged (mult/div/mthi/mtlo)
// where o represents some kind of operator

// assume no sa
#define spSll     0 // no rs, but sa
#define spSrl     2 // no rs, but sa
#define spSra     3	// no rs, but sa
#define spSllV    4
#define spSrlV    6
#define spSraV    7

#define spJR      8
#define spJAlR    9

#define spSysCall 12
#define spBreak   13

#define spMfHI    16 // only rd
#define spMtHI    17 // only rs
#define spMfLO    18 // only rd
#define spMtLO    19	// only rs
#define spMult    24 // no rd
#define spMultU   25	// no rd
#define spDiv     26 // no rd
#define spDivU    27	// no rd
#define spAdd     32	
#define spAddU    33
#define spSub     34
#define spSubU    35
#define spAnd     36
#define spOr      37
#define spXor     38
#define spNor     39
#define spSlt     42
#define spSltU    43

#define regZero 0
#define regAt 1
#define regV0 2
#define regV1 3
#define regA0 4
#define regA1 5
#define regA2 6
#define regA3 7
#define regT0 8
#define regT1 9
#define regT2 10
#define regT3 11
#define regT4 12
#define regT5 13
#define regT6 14
#define regT7 15
#define regS0 16
#define regS1 17
#define regS2 18
#define regS3 19
#define regS4 20
#define regS5 21
#define regS6 22
#define regS7 23
#define regT8 24
#define regT9 25
#define regK0 26
#define regK1 27
#define regGp 28
#define regSp 29
#define regS8 30
#define regRa 31
