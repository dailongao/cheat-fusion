char ITypeMnem[64][8]={ //tabel 1
"special","regimm","j","jal","beq","bne","blez","bgtz","addi","addiu","slti","sltiu","andi","ori","xori","lui",
"cop0","cop1","cop2","**","**","**","**","**","**","**","**","**","**","**","**","**",
"lb","lh","lwl","lw","lbu","lhu","lwr","**","sb","sh","swl","sw","**","**","swr","**",
"**","lwc1","lwc2","**","**","**","**","**","**","swc1","swc2","**","**","**","**","**"};

char RTypeMnem[64][8]={ //tabel 2
"sll","**","srl","sra","sllv","**","srlv","srav","jr","jalr","**","**","syscall","break","**","**",
"mfhi","mthi","mflo","mtlo","**","**","**","**","mult","multu","div","divu","**","**","**","**",
"add","addu","sub","subu","and","or","xor","nor","**","**","slt","sltu","**","**","**","**",
"**","**","**","**","**","**","**","**","**","**","**","**","**","**","**","**"} ;

char RITypeMnem[32][10] = {"bltz","bgez","**","**","**","**","**","**","**","**","**","**","**","**","**","**","bltzal","bgezal","**","**","**","**","**","**","**","**","**","**","**","**","**","**"};
char SysCoMnem[32][10] = {"mfc0","readi","writei","**","mtc0","**","writer","**","probe","**","**","**","**","**","**","**","rfex","**","**","**","**","**","**","**","**","**","**","**","**","**","**","**"};
char GTEMnem[32][12] = {"mfc2","gte_func1","cfc2","gte_func3","mtc2","gte_func5","ctc2","gte_func7","gte_func8","gte_func9","gte_func10","gte_func11","gte_func12","gte_func13","gte_func14","gte_func15","gte_func16","gte_func17","gte_func18","gte_func19","gte_func20","gte_func21","gte_func22","gte_func23","gte_func24","gte_func25","gte_func26","gte_func27","gte_func28","gte_func29","gte_func30","gte_func31"};

char CPURegStr[35][16]={"zero","at","v0","v1","a0","a1","a2","a3","t0","t1","t2",
"t3","t4","t5","t6","t7","s0","s1","s2","s3","s4","s5","s6","s7","t8","t9",
"k0","k1","gp","sp","fp","ra","PC","HI","LO"};

char SysCoRegStr[32][16]={"index","rand","2","bpc","4","bda","tar","dcic",
"8","bdam","10","bdpcm","sr","cause","epc","15",
"16","17","18","19","20","21","22","23",
"24","25","26","27","28","29","30","31"};

char GTERegStr[32][16]={"r11r12","r13r21","r22r23","r31r32","r33","trx","try","trz",
"l11l12","l13l21","l22l23","l31l32","l33","rbk","gbk","bbk",
"lr1lr2","lr3lg1","lg2lg3","lb1lb2","lb3","rfc","gfc","bfc",
"ofx","ofy","h","dqa","dqb","zsf3","zsf4","flag"};

char GTERegStr2[32][16]={"vxy0","vz0","vxy1","vz1","vxy2","vz2","rgb","otz",
"ir0","ir1","ir2","ir3","sxy0","sxy1","sxy2","sxy2p",
"sz0","sz1","sz2","sz3","rgb0","rgb1","rgb2","17",
"mac0","mac1","mac2","mac3","irgb","orgb","lzcs ","lzcr"}; 
