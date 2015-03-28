grammar NQL;

/*
 * Parser Rules
 */

compileUnit : element* EOF;

vector : LBRACKET (element)* RBRACKET;
list : LPAREN (element)* RPAREN;
set : '#' LCURLYBRACE (element)* RCURLYBRACE;
atom : OPERATOR | STRING | SYMBOL | NUMBER | KEYWORD;

element : atom | quotedList | list | vector | set;

quotedList : SINGLE_QUOTE list;

/*
 * Lexer Rules
 */
STRING :'"' ( '\\' . | ~('\\'|'"') )* '"';

SINGLE_QUOTE : '\'';

ALPHA : ('a'..'z') | ('A'..'Z');
SYMBOL_START : ALPHA;

OPERATOR : '+' | '-' | '*' | '/' | '.' | '=' | '%' | '^' | '&' |'!' | '|' | '<' | '>' | '>=' | '<=' | '!=' | '&&' | '||' | '~';
SYMBOL : (SYMBOL_START)+ (ALPHA | OPERATOR | DIGIT)*;
	
NUMBER : ('+' | '-')? (DIGIT)+ ('.' (DIGIT)+)?;

KEYWORD : ':' (ALPHA | DIGIT)+;
LPAREN : '(';
RPAREN : ')';

LBRACKET : '[';
RBRACKET : ']';

LCURLYBRACE : '{';
RCURLYBRACE : '}';
DOT : '.';

DIGIT : [0-9]+;
WS :	[ \t\r\n]+ -> skip;