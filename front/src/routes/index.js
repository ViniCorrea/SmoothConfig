import React from "react";
import {Route, Switch} from "react-router-dom";

import Tokenize from "./tokenize/index";

const App = ({match}) => (
  <div className="gx-main-content-wrapper">
    <Switch>
      <Route path={`${match.url}tokenize`} component={Tokenize}/>
    </Switch>
  </div>
);

export default App;
