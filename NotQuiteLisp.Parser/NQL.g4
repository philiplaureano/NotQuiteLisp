grammar NQL;

/*
 * Parser Rules
 */

compileUnit : element* EOF;

list : LPAREN (element)* RPAREN;

atom : OPERATOR | STRING | SYMBOL | NUMBER;

element : atom | list;

/*
 * Lexer Rules
 */
STRING :'"' ( '\\' . | ~('\\'|'"') )* '"'
	;

ALPHA : ('a'..'z') | ('A'..'Z');
SYMBOL_START : ALPHA;

OPERATOR : '+' | '-' | '*' | '/' | '.' | '#' | '=' | '%' | '^';
SYMBOL : (SYMBOL_START)+ (ALPHA | OPERATOR | DIGIT)*;
	
NUMBER : ('+' | '-')? (DIGIT)+ ('.' (DIGIT)+)?;

LPAREN : '(';
RPAREN : ')';
DOT : '.';

DIGIT : [0-9]+;
WS :	[ \t\r\n]+ -> skip;