import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from '../Layout';
import { Subscribe } from '../Subscribe';
import { Library } from '../Library';
import { Playlist } from '../Playlist';

import './index.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Subscribe} />
        <Route path='/playlist' component={Playlist} />
        <Route path='/library' component={Library} />
      </Layout>
    );
  }
}
