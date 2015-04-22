grammar NQL;

/*
 * Parser Rules
 */

compileUnit : element* EOF;

vector : LBRACKET (element)* RBRACKET;
list : LPAREN (element)* RPAREN;
set : '#' LCURLYBRACE (element)* RCURLYBRACE;
map : '{' (keyValuePair)+ (',')* keyValuePair* '}';
keyValuePair : atom element;

atom : OPERATOR | STRING | SYMBOL | NUMBER | KEYWORD;

element : quotedList | list | vector | map | set | atom;

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
COMMENT : ';' ~[\r\n]* '\r'? '\n' -> skip ;
DOT : '.';

DIGIT : [0-9]+;
WS :	[ \t\r\n]+ -> skip;