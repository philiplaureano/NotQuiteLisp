grammar NQL;

/*
 * Parser Rules
 */

compileUnit : element* EOF;

list : LPAREN (element)* RPAREN;

atom : DOT | STRING | NUMBER | SYMBOL;

element : atom | list;

/*
 * Lexer Rules
 */
STRING :'"' ( '\\' . | ~('\\'|'"') )* '"'
	;

SYMBOL_START : ('a'..'z') | ('A'..'Z');

OPERATOR : '+' | '-' | '*' | '/' | '.';
SYMBOL : (SYMBOL_START | OPERATOR | DIGIT)+;
	
NUMBER : ('+' | '-')? (DIGIT)+ ('.' (DIGIT)+)?;

LPAREN : '(';
RPAREN : ')';
DOT : '.';

DIGIT : [0-9]+;
WS :	[ \t\r\n]+ -> skip;