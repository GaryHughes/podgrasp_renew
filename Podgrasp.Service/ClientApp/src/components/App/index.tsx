import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from '../Layout';
import { Home } from '../Home';
import { Subscribe } from '../Subscribe';
import { Library } from '../Library';
import { Playlist } from '../Playlist';
import AuthorizeRoute from '../api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from '../api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from '../api-authorization/ApiAuthorizationConstants';

import './index.css'

export default class App extends Component {
   render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <AuthorizeRoute path='/playlist' component={Playlist} />
        <AuthorizeRoute path='/library' component={Library} />
        <AuthorizeRoute path='/subscribe' component={Subscribe} />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
      </Layout>
    );
  }
}
