# Draughts

This is a fairly naive implementation of English Draughts (AKA American Checkers) in C#, done for a in-house coding dojo at my workplace. It's not intended to be a playable game, it's just a demonstration of an OO design for a draughts game plus a very basic AI play routine. A console app is included that provides a simple UI for the game while two computer players play each other.

I designed this code for comprehension, not speed or simplicity. There are smaller, faster and better ways to store and process board state, find valid moves, and draw a UI. If you're looking for a complete human-playable game of Draughts, look elsewhere. If you're looking for code on which to base a 'proper' Draughts implementation, you might want to look elsewhere as well, although you're free to use this code if you like, subject to the terms of the license below (which is also found in the file license.txt in the root of this project).

Requires: .NET 4.5.2 - could probably be ported to .NET Core but I'm not doing that anytime soon...

LICENSE
-------

Copyright (c) 2016, Neil Hewitt

All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

