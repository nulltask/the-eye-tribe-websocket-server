
# The Eye Tribe WebSocket Server

The Eye Tribe WebSocket server written in C#. This server makes it possible to write your application on browser like Leap Motion SDK.

## Run

1. Install [The Eye Tribe SDK](https://theeyetribe.com/download/).
2. Connect your The Eye Tribe to PC.
3. Run EyeTribeServer (includes SDK).
4. Open this project as administration mode.
5. Build and run this project.
6. Connect to port 6556 via WebSocket protocol.
7. Write your application with browser!

## Example

Following code is JavaScript example.

    var ws = new WebSocket('ws://localhost:6556');
    
    ws.onmessage = function(e) {
      // {
      //   category: '...',
      //   request: '...',
      //   statuscode: ...,
      //   values: { ... }
      // } 
      console.log(JSON.parse(e.data));
    };

## Requirements

* The Eye Tribe 
* The Eye Tribe SDK
* .NET Framework 4.5
* Visual Studio 2013 for Windows Desktop

## Links

* [The Eye Tribe](http://theeyetribe.com/)
* [API Reference](http://dev.theeyetribe.com/api/)

## License

(The MIT License)

Copyright (c) 2014 Seiya Konno &lt;nulltask@gmail.com&gt;