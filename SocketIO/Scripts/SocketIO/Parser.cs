#region License
/*
 * Parser.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion
using UnityEngine;

namespace SocketIO
{
	public class Parser
	{
		public SocketIOEvent Parse(JSONObject json)
		{
			if (json.Count < 1) {
        #if SOCKET_IO_DEBUG
        Debug.Log("Invalid number of parameters received: " + json.Count);
        #endif
        throw new SocketIOException("Invalid number of parameters received: " + json.Count);
			}

			if (json[0].type != JSONObject.Type.STRING) {
        #if SOCKET_IO_DEBUG
        Debug.Log("Invalid parameter type. " + json[0].type + " received while expecting " + JSONObject.Type.STRING);
        #endif
				throw new SocketIOException("Invalid parameter type. " + json[0].type + " received while expecting " + JSONObject.Type.STRING);
			}

			if (json.Count == 1) {
        #if SOCKET_IO_DEBUG
        Debug.Log("json.Count == 1:");
        Debug.Log(json);
        #endif
				return new SocketIOEvent(json[0].str);
			} 

      if (json.Count > 2){
        // Create a JSON Object with the new fields
        #if SOCKET_IO_DEBUG
        Debug.Log("Message recieved as multiple parameters, parsing into object: ");
        #endif
        string JSONStringObject = "{";
        for (int i = 1; i < json.Count; i++)
        {
            if (i == json.Count - 1)
            {
                JSONStringObject += string.Format("\"{0}\":", i.ToString()) + json[i]; // "arg_1: "val", 
            }
            else
            {
                JSONStringObject += string.Format("\"{0}\":", i.ToString()) + json[i] + ","; // "arg_1: "val", 
            }
        }
        JSONStringObject += "}";

        return new SocketIOEvent(json[0].str, new JSONObject(JSONStringObject));
      }

			if (json[1].type == JSONObject.Type.OBJECT || json[1].type == JSONObject.Type.STRING) {
				return new SocketIOEvent(json[0].str, json[1]);
			} else {
        #if SOCKET_IO_DEBUG
        Debug.Log("Invalid argument type. " + json[1].type + " received while expecting " + JSONObject.Type.OBJECT);
        #endif
				throw new SocketIOException("Invalid argument type. " + json[1].type + " received");
			}
		}
	}
}
