import {Route, Switch} from 'react-router-dom';

import LoginPage from "./pages/LoginPage/LoginPage.tsx";
import RegistrationPage from "./pages/RegistrationPage/RegistrationPage.tsx";
import HomePage from "./pages/HomePage/HomePage.tsx";

const Router = () => (
  <Switch>
    <Route exact path="/" component={HomePage}/>
    <Route exact path="/welcome" component={welcome}/>
    <Route path="/login" component={LoginPage}/>
    <Route path="/reg" component={RegistrationPage}/>
  </Switch>
)
const welcome = () => <><h1 className="text-3xl font-bold underline">Hello world!</h1><a href="/auth">Login</a></>;

  export default Router;
