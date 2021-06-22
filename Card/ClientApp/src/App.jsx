import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { OpenCard } from './components/OpenCard';
import { CloseCard } from './components/CloseCard';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/open-card' component={OpenCard} />
        <Route path='/close-card' component={CloseCard} />
      </Layout>
    );
  }
}
