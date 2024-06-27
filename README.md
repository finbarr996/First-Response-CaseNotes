# CaseNotes 2.18.4.3
The purpose of CaseNotes is to provide a single lightweight application program to run on the Microsoft Windows platform to allow incident responders & forensic analysts and examiners of any discipline to securely record their contemporaneous notes electronically.
This is especially useful as more and more jurisdictions are enforcing regulatory standards for investigations, like ISO17025 in the United Kingdom.
CaseNotes stores your notes, checklists and screen captures in a SQLite database that can optionally be encrypted for additional security. 
It has been used by thousands of forensic analysts around the world since 2003 – we’re very excited to finally release this new version, which has so many new features, based on user requests.

## The main features are:
lexible configuration of case meta-data (case details, like the reference number, etc.)
Secure ‘write-once, read-many’ style of case note data capture and storage
Full audit trail of case note data entry and metadata edits in a self-contained log
Spell checker built in, with the ability to add/delete words from the dictionary
A GUI interface builder for creating multiple checklists
A templating function for storing text snippets for often entered phrases
Tamper evident storage of data using internal MD5 hashes for all data entered
No heavy database technologies – all you need is the program and your SQLite case file
Use of AES 512bit encryption (optional) to further secure data in sensitive cases
Storage of configuration information in a user editable text based .xml file
Support for running multiple copies of CaseNotes at the same time
Compatible with non-roman and double-byte character sets (Japanese, Russian, Greek, Chinese, Korean, Arabic …)

## What CaseNotes is Not:
CaseNotes is NOT a multi-user application – each analyst should keep their own notes in their own Case File even when working on the same investigation. CaseNotes is not designed to support simultaneous multi-user access and bad things will happen if you attempt this – so don’t!

## CaseNotes is NOT a case management system – it’s just for recording your notes – nothing more, nothing less.

I've decided to open-source the code for CaseNotes - so you are welcome to download the source code, modify it and use it without restriction.
This is written in C# using Visual Studio and has a number of add-in libraries for doing things like spell checking, and printing.
If you want the last compiled executable, you can find that under releases - the most up-to-date version is 2.18.4.3.
