using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Metaproject.Forms
{
    public class EventDelayer
    {

        #region <opis dzialania>
        // klasa sluzaca do opoznienia wykonania zdarzenia o okreslona w konstruktorze
        // liczbe sekund. Mozna ja wykorzystac przy wstrzymania wyszukiwania do czasu
        // kiedy uzytkownik przestanie wklepywac dane.
        #endregion

        #region <nested types>

        /// <summary>
        /// typ sluzacy do okreslenia aktualnego stanu hand
        /// </summary>
        enum State
        {
            Idle,
            Busy,
        }
        #endregion

        #region <prywatne pola>
        /// <summary>
        /// Flaga sledzaca stan obiektu
        /// </summary>
        State _currentState = State.Idle;

        /// <summary>
        /// czasomierz odmierzajacy czas od ostatniej notyfikacji
        /// </summary>
        Timer _timer = new Timer();

        /// <summary>
        /// ref do uruchamianej metody po uplynieciu czasu
        /// </summary>
        EventHandler _handlerToDelay;

        /// <summary>
        /// aktualne argumenty metody
        /// </summary>
        object _sender;
        EventArgs _e;
        #endregion

        #region <ctor>
        public EventDelayer(int delayInSeconds, EventHandler handlerToDelay)
        {
            _timer.Interval = delayInSeconds * 1000;
            _timer.Tick += OnTimerTick;
            _currentState = State.Idle;
            _handlerToDelay = handlerToDelay;
        }
        #endregion

        #region <pub>
        public void Notify(object sender, EventArgs e)
        {

            if (_currentState == State.Busy) return;

            _sender = sender;
            _e = e;

            // resetowanie timera
            _timer.Stop();
            _timer.Start();

        }
        #endregion

        #region <handlery>
        /// <summary>
        /// handler odpalajacy opozniana metode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnTimerTick(object sender, EventArgs e)
        {
            _timer.Stop();

            if (_currentState == State.Busy) return;

            _currentState = State.Busy;

            if (null != _handlerToDelay)
                _handlerToDelay(_sender, _e);

            _currentState = State.Idle;
        }
        #endregion

    }
}
