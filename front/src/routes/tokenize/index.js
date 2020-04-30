import React from "react";
import {Route, Switch} from "react-router-dom";
import Dashboard from "./dashboard";
import Tokens from "./tokens";
import Login from "./login";

const Tokenize = ({match}) => (
  <Switch>
    <Route path={`${match.url}/dashboard`} component={Dashboard}/>
    <Route path={`${match.url}/tokens`} component={Tokens}/>
    <Route path={`${match.url}/login`} component={Login}/>
  </Switch>
);

export default Tokenize;
