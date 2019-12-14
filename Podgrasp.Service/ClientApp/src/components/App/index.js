import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from '../Layout';
import { Subscribe } from '../Subscribe';
import { Library } from '../Library';
import { Playlist } from '../Playlist';
import AuthorizeRoute from '../api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from '../api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from '../api-authorization/ApiAuthorizationConstants';

import './index.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Subscribe} />
        <AuthorizeRoute path='/playlist' component={Playlist} />
        <AuthorizeRoute path='/library' component={Library} />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
      </Layout>
    );
  }
}
