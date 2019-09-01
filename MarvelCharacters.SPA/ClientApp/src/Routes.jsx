import React from "react";
import { BrowserRouter, Route, Link, Switch } from "react-router-dom";
import SignInPage from './pages/SignInPage';
import SignUpPage from './pages/SignUpPage';
import HomePage from './pages/HomePage';

export default function Routes() {
    return (
        <BrowserRouter>
        <div>
            <ul>
                <li>
                    <Link to="/">Home</Link>
                </li>
                <li>
                    <Link to="/signup">SignUp</Link>
                </li>
                <li>
                    <Link to="/signin">SignIn</Link>
                </li>
            </ul>
            <hr />
            <Switch>
                <Route exact path="/" component={props => <HomePage {...props} />} />
                <Route exact path="/signin" component={props => <SignInPage {...props} />} />
                <Route path="/signup" component={props => <SignUpPage {...props} />} />
            </Switch>
        </div>
        </BrowserRouter>
    );
}