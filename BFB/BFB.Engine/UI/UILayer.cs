using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Content;
using BFB.Engine.Event;
using BFB.Engine.Scene;
using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.UI
{
    /// <summary>
    /// Used to create a layer for ui elements
    /// </summary>
    public abstract class UILayer
    {
        #region Properties

        /// <summary>
        /// The identification key for the uiLayer
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// The UIManager for the UILayer
        /// </summary>
        public static UIManager UIManager { get; set; }

        /// <summary>
        /// The SceneManager class
        /// </summary>
        public static SceneManager SceneManager { get; set; }

        /// <summary>
        /// The global event manager
        /// </summary>
        public static EventManager<GlobalEvent> GlobalEventManager { get; set; }

        /// <summary>
        /// Used to indicate if the scene should be drawn in debug mode
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Represents the foundation of the UI
        /// </summary>
        public UIRootComponent RootUI { get; set; }

        /// <summary>
        /// The scene that started the UILayer
        /// </summary>
        protected Scene.Scene ParentScene { get; set; }

        /// <summary>
        /// Blocks all input events from propagating after the uiLayer
        /// </summary>
        public bool BlockInput { get; set; }

        /**
         * Indicates the current focus position of the tabIndex
         */
        private int? _tabPosition;

        /**
         * Contains the elements that can be tabbed or focused
         */
        private readonly List<UIComponent> _tabIndex;

        /**
         * Contains the elements that have events tied to them
         */
        private readonly List<UIComponent> _eventIndex;

        /**
         * Contains elements that may need to be updated over time
         */
        private readonly List<UIComponent> _updateIndex;

        //Event stuff
        private readonly List<int> _eventGlobalListenerIds;
        private readonly Dictionary<string, List<Action<InputEvent>>> _inputEventListeners;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a UILayer for displaying UIElements
        /// </summary>
        /// <param name="key"></param>
        protected UILayer(string key)
        {
            Key = key;
            RootUI = null;
            Debug = false;
            BlockInput = false;
            _tabPosition = null;

            _tabIndex = new List<UIComponent>();
            _eventIndex = new List<UIComponent>();
            _updateIndex = new List<UIComponent>();
            _eventGlobalListenerIds = new List<int>();
            _inputEventListeners = new Dictionary<string, List<Action<InputEvent>>>();
        }

        #endregion

        #region Initilize Root

        /// <summary>
        /// Initializes the layer for drawing
        /// </summary>
        /// <param name="rootNode"></param>
        public void InitializeRoot(UIRootComponent rootNode)
        {
            _eventIndex.Clear();
            _tabIndex.Clear();
            RootUI = rootNode;
        }

        #endregion

        #region ProcessEvents

        /// <summary>
        /// Processes Input Events into UIEvents
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        public bool ProcessEvents(IEnumerable<UIEvent> events)
        {
            bool eventAccepted = false;

            //loop through all ui events
            foreach (UIEvent uiEvent in events)
            {
                #region Reset Component Attributes

                //loop through all components in the event index and reset attributes
                foreach (UIComponent component in _eventIndex)
                {
                    component.RenderAttributes =
                        component.DefaultAttributes.CascadeAttributes(component.DefaultAttributes);

                    if (component.RenderAttributes.Position == Position.Relative)
                    {
                        component.RenderAttributes.X =
                            component.RenderAttributes.OffsetX + component.Parent?.RenderAttributes.X ?? 0;
                        component.RenderAttributes.Y =
                            component.RenderAttributes.OffsetY + component.Parent?.RenderAttributes.Y ?? 0;
                    }
                }

                #endregion

                #region Unfocus With click

                if (uiEvent.EventKey == "click")
                {
                    if (_tabPosition != null && _tabPosition > 0 && _tabPosition < _tabIndex.Count)
                    {
                        _tabIndex[(int) _tabPosition].Focused = false;
                        _tabPosition = null;
                    }
                }

                #endregion

                if (uiEvent.EventKey == "click" || uiEvent.EventKey == "mouseup" || uiEvent.EventKey == "hover" ||
                    uiEvent.EventKey == "mousescroll")
                {
                    
                    //Get any event elements that the mouse is on top of
                    List<UIComponent> candidateEventComponent = _eventIndex.Where(c =>
                            c.RenderAttributes.X <= uiEvent.Mouse.X
                            && c.RenderAttributes.X + c.RenderAttributes.Width >= uiEvent.Mouse.X
                            && c.RenderAttributes.Y <= uiEvent.Mouse.Y
                            && c.RenderAttributes.Y + c.RenderAttributes.Height >= uiEvent.Mouse.Y)
                        .OrderBy(x => x.RenderAttributes.Width * x.RenderAttributes.Height)
                        .ToList();

                    //
                    foreach (UIComponent component in _eventIndex.Where(x => !candidateEventComponent.Contains(x)))
                    {
                        if (!component.IsHovered) continue;
                        
                        UIEvent leaveEvent = new UIEvent(uiEvent)
                        {
                            EventKey = "mouseleave"
                        };

                        component.ProcessEvent(leaveEvent);
                        component.IsHovered = false;
                    }

                    //stop here if no elements were found
                    if (!candidateEventComponent.Any())
                        continue;

                    #region Focus with click

                    if (uiEvent.EventKey == "click")
                        if (candidateEventComponent[0].Focusable)
                        {
                            candidateEventComponent[0].Focused = true;
                            _tabPosition = _tabIndex.FindIndex(x => x == candidateEventComponent[0]);
                        }

                    #endregion

                    #region Process Events on each component

                    //Start processing each component
                    foreach (UIComponent component in candidateEventComponent)
                    {
                        if (!component.IsHovered)
                        {
                            UIEvent leaveEvent = new UIEvent(uiEvent)
                            {
                                EventKey = "mouseenter"
                            };
                            component.ProcessEvent(leaveEvent);
                        }
                        
                        component.IsHovered = true;
                        uiEvent.Component = component;

                        //Process event here
                        component.ProcessEvent(uiEvent);

                        if (!uiEvent.Propagate())
                            eventAccepted = true;
                    }

                    #endregion
                }
                else
                {
                    //Key events

                    #region Unfocus element with Escape

                    if (uiEvent.Keyboard.KeyboardState.IsKeyDown(Keys.Escape))
                    {
                        if (_tabPosition != null)
                            _tabIndex[(int) _tabPosition].Focused = false;

                        _tabPosition = null;
                    }

                    #endregion

                    #region Cycle Focused elements with tab/tab+lshift

                    if (uiEvent.EventKey == "focus" && _tabIndex.Any())
                    {
                        //Unfocus previous element
                        if (_tabPosition != null)
                            _tabIndex[(int) _tabPosition].Focused = false;


                        if (_tabPosition == null) //Default first position
                            _tabPosition = 0;
                        else if (uiEvent.Keyboard.KeyboardState.IsKeyDown(Keys.LeftShift))
                            _tabPosition--; //shift + tab
                        else
                            _tabPosition++; //tab

                        if (_tabPosition > _tabIndex.Count - 1)
                            _tabPosition = 0;
                        else if (_tabPosition < 0)
                            _tabPosition = _tabIndex.Count - 1;

                        _tabIndex[(int) _tabPosition].Focused = true;
                    }

                    #endregion

                    //Key events require a focused element
                    if (!_tabIndex.Any())
                        continue;

                    if (_tabPosition == null)
                        continue;

                    uiEvent.Component = _tabIndex[(int) _tabPosition];

                    _tabIndex[(int) _tabPosition].ProcessEvent(uiEvent);

                    if (!uiEvent.Propagate())
                        eventAccepted = true;
                }
            }

            return eventAccepted;
        }

        #endregion

        #region AddEventComponent

        /// <summary>
        /// Adds a UIComponents that uses events into the Event Index
        /// </summary>
        /// <param name="component">The UIComponent that will emit events</param>
        public void AddEventComponent(UIComponent component)
        {
            _eventIndex.Add(component);
        }

        #endregion

        #region AddTabIndexComponent

        /// <summary>
        /// Adds a UIComponent to the tabindex if it can be tabbed
        /// </summary>
        /// <param name="component">The UIComponent that can be tabbed</param>
        public void AddTabIndexComponent(UIComponent component)
        {
            _tabIndex.Add(component);
        }

        #endregion

        #region AddUpdateIndexComponent

        public void AddUpdateIndexComponent(UIComponent component)
        {
            _updateIndex.Add(component);
        }

        #endregion

        #region AddGlobalListener

        /// <summary>
        /// Allows for adding of global event listeners that are automatically disposed of when the UILayer is stopped
        /// </summary>
        /// <param name="eventKey">The event to listen for</param>
        /// <param name="eventHandler">The event handler to perform an action when a event is received</param>
        protected void AddGlobalListener(string eventKey, Action<GlobalEvent> eventHandler)
        {
            _eventGlobalListenerIds.Add(GlobalEventManager.AddEventListener(eventKey, eventHandler));
        }

        #endregion

        #region ProcessInputEvent

        public bool ProcessInputEvent(InputEvent inputEvent)
        {
            if (!_inputEventListeners.ContainsKey(inputEvent.EventKey))
                return false;

            foreach (Action<InputEvent> action in _inputEventListeners[inputEvent.EventKey])
            {
                action?.Invoke(inputEvent);

                if (!inputEvent.Propagate())
                    return true;
            }

            return false;
        }

        #endregion

        #region AddInputListener

        /// <summary>
        /// Allows for adding of input event listeners that are automatically disposed of when the UILayer is stopped
        /// </summary>
        /// <param name="eventKey">The event to listen for</param>
        /// <param name="eventHandler">The event handler to perform an action when a event is received</param>
        protected void AddInputListener(string eventKey, Action<InputEvent> eventHandler)
        {
            if (_inputEventListeners.ContainsKey(eventKey))
            {
                _inputEventListeners[eventKey].Add(eventHandler);
            }
            else
            {
                _inputEventListeners.Add(eventKey, new List<Action<InputEvent>>());
                _inputEventListeners[eventKey].Add(eventHandler);
            }
        }

        #endregion

        #region UpdateLayer

        public void UpdateLayer(GameTime time)
        {
            Update(time);

            foreach (UIComponent uiComponent in _updateIndex.ToList())
                uiComponent.Update(time);
        }

        #endregion

        #region Start

        public void Start(Scene.Scene parentScene)
        {
            ParentScene = parentScene;
            Init();
        }

        #endregion

        #region Stop

        /// <summary>
        /// Called when a UILayer needs to be stopped
        /// </summary>
        public void Stop()
        {
            ParentScene = null;
            RootUI = null;
            _tabPosition = null;

            _eventIndex.Clear();
            _tabIndex.Clear();
            _updateIndex.Clear();

            foreach (int id in _eventGlobalListenerIds)
                GlobalEventManager?.RemoveEventListener(id);

            _inputEventListeners.Clear();
        }

        #endregion

        /// <summary>
        /// An optional init method for a UILayer to use for setup. Called every time a UILayer is started
        /// </summary>
        protected virtual void Init()
        {
        }

        /// <summary>
        /// An optional method for updating anything that may happen in the uiLayer
        /// </summary>
        /// <param name="time"></param>
        protected virtual void Update(GameTime time)
        {
        }

        /// <summary>
        /// Optional Render method for rendering things on top of the ui
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="content"></param>
        public virtual void Draw(SpriteBatch graphics, BFBContentManager content)
        {
        }

        /// <summary>
        /// The area to define the UIComponent element layouts
        /// </summary>
        public abstract void Body();
        
        
    }
}