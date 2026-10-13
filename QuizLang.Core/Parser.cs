using QuizLang.Core.Modelo;



using System;

namespace QuizLang.Core {



public class Parser {
	public const int _EOF = 0;
	public const int _letra = 1;
	public const int _number = 2;
	public const int _string = 3;
	public const int maxT = 18;

	const bool _T = true;
	const bool _x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public SymbolTable symbolTable;
  public Examen Examen => symbolTable?.Examen;

  private static string Unquote(string s) {
      if (string.IsNullOrEmpty(s)) return "";
      if (s.StartsWith("\"") && s.EndsWith("\"") && s.Length >= 2) {
          s = s.Substring(1, s.Length - 2);
      }
      return s.Replace("\\\"", "\"")
              .Replace("\\\\", "\\")
              .Replace("\\n", "\n")
              .Replace("\\r", "\r")
              .Replace("\\t", "\t");
  }



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
			if (la.kind == maxT) {
				SynErr(maxT);
			}
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

	
	void QuizLang() {
		symbolTable = new SymbolTable(errors); 
		Expect(4);
		Expect(3);
		symbolTable.SetExamen(Unquote(t.val), t.line, t.col); 
		if (la.kind == 5) {
			Config();
		}
		while (la.kind == 6 || la.kind == 12 || la.kind == 14) {
			Pregunta();
		}
		symbolTable.ValidarExamen(); 
	}

	void Config() {
		Expect(5);
		Expect(2);
		int puntajeMin = int.Parse(t.val);
		symbolTable.SetConfig(puntajeMin, t.line, t.col);
		
	}

	void Pregunta() {
		if (la.kind == 6) {
			Multiple();
		} else if (la.kind == 12) {
			Texto();
		} else if (la.kind == 14) {
			VerdaderoFalso();
		} else SynErr(19);
	}

	void Multiple() {
		var pm = new PreguntaMultiple();
		pm.Puntos = 1;
		Opcion op;
		
		Expect(6);
		Expect(3);
		pm.Enunciado = Unquote(t.val);
		pm.Line = t.line;
		pm.Col = t.col;
		
		if (la.kind == 7) {
			Get();
			Expect(2);
			pm.Puntos = int.Parse(t.val); 
		}
		Expect(8);
		Opcion(out op);
		pm.Opciones.Add(op); 
		while (la.kind == 1) {
			Opcion(out op);
			pm.Opciones.Add(op); 
		}
		Expect(9);
		symbolTable.ValidarYAgregarPreguntaMultiple(pm); 
	}

	void Texto() {
		var pt = new PreguntaTexto();
		pt.Puntos = 1;
		
		Expect(12);
		Expect(3);
		pt.Enunciado = Unquote(t.val);
		pt.Line = t.line;
		pt.Col = t.col;
		
		if (la.kind == 7) {
			Get();
			Expect(2);
			pt.Puntos = int.Parse(t.val); 
		}
		if (la.kind == 13) {
			Get();
			Expect(3);
			pt.RespuestaCorrecta = Unquote(t.val); 
		}
		symbolTable.ValidarYAgregarPreguntaTexto(pt); 
	}

	void VerdaderoFalso() {
		var pvf = new PreguntaVF();
		pvf.Puntos = 1;
		
		Expect(14);
		Expect(3);
		pvf.Enunciado = Unquote(t.val);
		pvf.Line = t.line;
		pvf.Col = t.col;
		
		if (la.kind == 7) {
			Get();
			Expect(2);
			pvf.Puntos = int.Parse(t.val); 
		}
		Expect(15);
		if (la.kind == 16) {
			Get();
			pvf.RespuestaCorrecta = true; 
		} else if (la.kind == 17) {
			Get();
			pvf.RespuestaCorrecta = false; 
		} else SynErr(20);
		symbolTable.ValidarYAgregarPreguntaVF(pvf); 
	}

	void Opcion(out Opcion op) {
		op = new Opcion();
		
		Expect(1);
		op.Letra = t.val[0]; op.Line = t.line; op.Col = t.col; 
		Expect(10);
		Expect(3);
		op.Texto = Unquote(t.val); 
		if (la.kind == 11) {
			Get();
			op.EsCorrecta = true; 
		}
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		QuizLang();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x}

	};
} // end Parser


public class Errors {
	public class ErrorItem {
		public int Line { get; set; }
		public int Column { get; set; }
		public string Message { get; set; } = "";
		public bool IsWarning { get; set; }
		public override string ToString() => $"linea {Line} col {Column}: {Message}";
	}

	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "linea {0} col {1}: {2}";   // 0=line, 1=column, 2=text
	public System.Collections.Generic.List<ErrorItem> Items = new System.Collections.Generic.List<ErrorItem>();

	private static string TraducirMensaje(string s) {
		if (s == "??? expected") return "caracter o simbolo lexico no reconocido";
		if (s == "EOF expected") return "se esperaba fin de archivo";
		if (s.EndsWith(" expected")) return "se esperaba " + s.Substring(0, s.Length - " expected".Length);
		if (s.StartsWith("invalid ")) return "simbolo no valido: " + s.Substring("invalid ".Length);
		if (s.Contains("not expected")) return "simbolo inesperado";
		return s;
	}

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "letra expected"; break;
			case 2: s = "number expected"; break;
			case 3: s = "string expected"; break;
			case 4: s = "\"EXAMEN\" expected"; break;
			case 5: s = "\"PUNTAJE_MINIMO\" expected"; break;
			case 6: s = "\"PREGUNTA_MULTIPLE\" expected"; break;
			case 7: s = "\"PUNTOS\" expected"; break;
			case 8: s = "\"{\" expected"; break;
			case 9: s = "\"}\" expected"; break;
			case 10: s = "\")\" expected"; break;
			case 11: s = "\"[CORRECTA]\" expected"; break;
			case 12: s = "\"PREGUNTA_TEXTO\" expected"; break;
			case 13: s = "\"RESPUESTA\" expected"; break;
			case 14: s = "\"PREGUNTA_VF\" expected"; break;
			case 15: s = "\"=\" expected"; break;
			case 16: s = "\"VERDADERO\" expected"; break;
			case 17: s = "\"FALSO\" expected"; break;
			case 18: s = "??? expected"; break;
			case 19: s = "invalid Pregunta"; break;
			case 20: s = "invalid VerdaderoFalso"; break;

			default: s = "error " + n; break;
		}
		s = TraducirMensaje(s);
		Items.Add(new ErrorItem { Line = line, Column = col, Message = s, IsWarning = false });
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		Items.Add(new ErrorItem { Line = line, Column = col, Message = s, IsWarning = false });
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		Items.Add(new ErrorItem { Line = 0, Column = 0, Message = s, IsWarning = false });
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		Items.Add(new ErrorItem { Line = line, Column = col, Message = s, IsWarning = true });
		errorStream.WriteLine("advertencia: " + errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		Items.Add(new ErrorItem { Line = 0, Column = 0, Message = s, IsWarning = true });
		errorStream.WriteLine("advertencia: " + s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
}