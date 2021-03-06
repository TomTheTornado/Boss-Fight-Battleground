using System.Collections.Generic;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.Event
{
    public class UIEvent : InputEvent
    {
        /// <summary>
        /// The UI componenet that this event will affect
        /// </summary>
        public UIComponent Component { get; set; }

        /// <summary>
        /// Creates a new UIEvent
        /// </summary>
        /// <param name="inputEvent">The user input that will control this event</param>
        public UIEvent(InputEvent inputEvent)
        {
            EventId = inputEvent.EventId;
            Keyboard = inputEvent.Keyboard;
            Mouse = inputEvent.Mouse;
        }

        /// <summary>
        /// Takes the InputEvent and changes it into a UIEvent and adds it to the List of UIEvents
        /// </summary>
        /// <param name="inputEvent">Event to be converted</param>
        /// <returns>An updated version of the list of UIEvents</returns>
        public static List<UIEvent> ConvertInputEventToUIEvent(InputEvent inputEvent)
        {
            List<UIEvent> eventList = new List<UIEvent>();
            
            //Using existing input events to generate the new event type
            switch (inputEvent.EventKey)
            {
                case "mousemove":
                    //hover event
                    eventList.Add(new UIEvent(inputEvent)
                    {
                        EventKey = "hover"
                    });
                    //enter
                    //exit
                    
                    break;
                case "mouseclick":
                    
                    //click event
                    eventList.Add(new UIEvent(inputEvent)
                    {
                        EventKey = "click"
                    });
                    
                    break;
                case "mouseup":
                    
                    //click event
                    eventList.Add(new UIEvent(inputEvent)
                    {
                        EventKey = "mouseup"
                    });
                    
                    break;
                case "keypress":
                    
                    //keypress, focus, unfocus
                    eventList.Add(new UIEvent(inputEvent)
                    {
                        EventKey = "keypress"
                    });
                    
                    //only changes focus with tab
                    if (inputEvent.Keyboard.KeyEnum == Keys.Tab)
                    {
                        eventList.Add(new UIEvent(inputEvent)
                        {
                            EventKey = "focus"
                        });
                    }

                    break;
                case "keyup":
                    
                    //keyup
                    eventList.Add(new UIEvent(inputEvent)
                    {
                        EventKey = "keyup"
                    });
                    
                    break;
                case "keydown":
                    
                    //keydown
                    eventList.Add(new UIEvent(inputEvent)
                    {
                        EventKey = "keydown"
                    });
                    
                    break;
                case "mousescroll":
                    eventList.Add(new UIEvent(inputEvent)
                    {
                        EventKey = "mousescroll"
                    });
                    break;
            }

            return eventList;
        }
    }
}