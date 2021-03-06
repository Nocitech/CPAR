﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication
{
    public class Destuffer
    {
        private enum State
        {
            WAITING_FOR_DLE,
            WAITING_FOR_STX,
            RECEIVING_DATA,
            WAITING_FOR_ETX
        }


        public event Action<Destuffer, byte[]> OnReceive;

        public void Reset()
        {
            state = State.WAITING_FOR_DLE;
            buffer.Clear();
        }

        public void Add(byte data)
        {
            switch (state)
            {
                case State.WAITING_FOR_DLE:
                    HandleWaitingForDLE(data);
                    break;

                case State.WAITING_FOR_STX:
                    HandleWaitingForSTX(data);
                    break;

                case State.RECEIVING_DATA:
                    HandleReceivingData(data);
                    break;
                case State.WAITING_FOR_ETX:
                    HandleWaitingForETX(data);
                    break;
            }
        }

        private void HandleWaitingForDLE(byte data)
        {
            if (data == Frame.DLE)
                state = State.WAITING_FOR_STX;
        }

        private void HandleWaitingForSTX(byte data)
        {
            if (data == Frame.STX)
            {
                state = State.RECEIVING_DATA;
                buffer.Clear();
            }
            else if (data != Frame.DLE)
                state = State.WAITING_FOR_DLE;
        }

        private void HandleReceivingData(byte data)
        {
            if (data != Frame.DLE)
                buffer.Add(data);
            else
                state = State.WAITING_FOR_ETX;
        }

        private void HandleWaitingForETX(byte data)
        {
            if (data == Frame.DLE)
            {
                buffer.Add(Frame.DLE);
                state = State.RECEIVING_DATA;
            }
            else if (data == Frame.ETX)
            {
                state = State.WAITING_FOR_DLE;
                NotifyListeners();
            }
            else if (data == Frame.STX)
            {
                state = State.RECEIVING_DATA;
                buffer.Clear();
            }
            else
            {
                state = State.WAITING_FOR_DLE;
            }
        }

        private void NotifyListeners()
        {
            if (OnReceive != null)
                OnReceive(this, buffer.ToArray());
        }

        private State state = State.WAITING_FOR_DLE;
        private List<byte> buffer = new List<byte>();
    }
}
