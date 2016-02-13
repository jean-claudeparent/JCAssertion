# JCAssertion
Utilitaire pour tester l'état d'un environnement de test sous windows

Première version par Jean-Claude Parent

==================

Ce progranne permet de vérifier des assertions sur l'état de diverses ressiyrces
informatiques.

Par exemple à terme il permettra de vérifier qu'un fichier existe sur le disque dur d'un ordinateur
ou qu'une table existe dans une base de données  oracle

============
Arguments d'ebtrée 

JC
Assertion /Fichier="c:App.xml" /Variable="c:valeurs.xml"

/Fichier=
Est le fichier xml qui contient lesassertions à vérifier

/Variable=
est un fichier xml qyu cibtuebt des clés et des valeurs à substituer dans le fichier d'assertion. Ce fichier est nécessaire 
pour que JCAssertion puisse être utilisé par un test runner propriétaire

======

Traitement

Le fichier des assertions est lu, les valeurs sont substituées
en conformité avec le fichier de valeur.
Chaque assertion est évaluée
et son résultat est écrit dans un fochier .log du
même nom que le fichier d'assertion.
Si au moins une assertion est fausse, le programme retourne un 
code de retour du système d'exploitation `la valeur 1

Si une excepion non prévue est rencontrée le
code de reour 99 est retourné.
