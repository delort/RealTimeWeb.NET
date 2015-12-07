'use strict';

import React from 'react';
import ReactDom from 'react-dom';
import { compose, createStore, applyMiddleware } from 'redux';
import { Provider } from 'react-redux';
import { devTools, persistState } from 'redux-devtools';
import thunk from 'redux-thunk';
import { DevTools, DebugPanel, LogMonitor } from 'redux-devtools/lib/react';
import { reduxReactRouter } from 'redux-router';
import { createHistory } from 'history';

import membership from './api/membership';
import reducers from './state/reducers';
import dispatcher from './state/dispatcher';

import Router from './router';

const storeFactory = compose(
  applyMiddleware(thunk),
  reduxReactRouter({ createHistory }),
  devTools(),
  persistState(window.location.href.match(/[?&]debug_session=([^&]+)\b/))
)(createStore);

const store = storeFactory(reducers);

dispatcher.set(store.dispatch);

let contentElement = document.getElementById('application-content');

ReactDom.render((
    <div>
        <Provider store={store}>
            <Router />
        </Provider>     
        <DebugPanel top right bottom>
            <DevTools store={store} monitor={LogMonitor} />
        </DebugPanel>
    </div>
), contentElement);

membership.initialize();

