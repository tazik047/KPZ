
using System;



public class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _const = 2;
	public const int _operat = 3;
	public const int _neg = 4;
	public const int maxT = 9;

	const bool _T = true;
	const bool _x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;



	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void Sample() {
		while (StartOf(1)) {
			D();
			Expect(5);
		}
	}

	void D() {
		M();
		B();
	}

	void M() {
		if (la.kind == 4) {
			Get();
			P();
		} else if (la.kind == 1 || la.kind == 2 || la.kind == 6) {
			P();
		} else SynErr(10);
	}

	void B() {
		if (la.kind == 3) {
			Get();
			M();
			B();
		} else if (la.kind == 5 || la.kind == 7 || la.kind == 8) {
			if (false) {
			}
		} else SynErr(11);
	}

	void P() {
		if (la.kind == 6) {
			Get();
			D();
			Expect(7);
		} else if (la.kind == 1) {
			L();
		} else if (la.kind == 2) {
			Get();
		} else SynErr(12);
	}

	void L() {
		Expect(1);
		Pred();
	}

	void Pred() {
		if (la.kind == 6) {
			Get();
			InPred();
			Expect(7);
		} else if (StartOf(2)) {
			if (false) {
			}
		} else SynErr(13);
	}

	void InPred() {
		D();
		E();
	}

	void E() {
		if (la.kind == 8) {
			Get();
			InPred();
		} else if (la.kind == 7) {
			if (false) {
			}
		} else SynErr(14);
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Sample();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_T,_x, _T,_x,_T,_x, _x,_x,_x},
		{_x,_x,_x,_T, _x,_T,_x,_T, _T,_x,_x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "const expected"; break;
			case 3: s = "operat expected"; break;
			case 4: s = "neg expected"; break;
			case 5: s = "\"#\" expected"; break;
			case 6: s = "\"(\" expected"; break;
			case 7: s = "\")\" expected"; break;
			case 8: s = "\",\" expected"; break;
			case 9: s = "??? expected"; break;
			case 10: s = "invalid M"; break;
			case 11: s = "invalid B"; break;
			case 12: s = "invalid P"; break;
			case 13: s = "invalid Pred"; break;
			case 14: s = "invalid E"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
