import React, { useState, useEffect } from 'react';
import { Link, useHistory } from 'react-router-dom';
// import { ReactComponent as Logo } from '../../UI/Images/Logo.svg';
import './LoginPage.css';
import AuthService from '../../services/api/auth.service';
import { StyledInput } from "../../components/StyledInput";
import BlueButton from "../../UI/Buttons/blueButton";

const LoginPage = () => {
  const history = useHistory();
  const [loginError, setLoginError] = useState(false);
  const handleSubmit = () => {
    AuthService.login(email, password).then(
      () => {
        history.push("/");
      },
    ).catch((error) => {
      setLoginError(true)
    });
  };
  const [email, setEmail] = useState();
  const [password, setPassword] = useState();
  const [usernameError, setUsernameError] = useState('');
  const [passwordError, setPasswordError] = useState('');
  const [formValid, setFormValid] = useState(false)

  useEffect(() => {
    if (usernameError || passwordError) {
      setFormValid(false)
    } else {
      setFormValid(true)
    }
  }, [usernameError, passwordError])

  const emailHandler = (e) => {
    setEmail(e.target.value)
    if (!e.target.value) {
      setUsernameError('Email не может быть пустым')
    } else {
      setUsernameError('')
    }
  }

  const passwordHandler = (e) => {
    setPassword(e.target.value)
    if (e.target.value.length < 8) {
      setPasswordError('Пароль должен быть не менее 8 символов')
      if (!e.target.value) {
        setPasswordError('Пароль не может быть пустым')
      }
    } else {
      setPasswordError('')
    }
  }

  return (
    <div className="auth">
      <div className="authForm">
        {/*<Link to="/"><Logo className="logotype"/></Link>*/}
        <div className="inputWrapper">
          <StyledInput
            onChange={emailHandler}
            error={usernameError}
            id={'email'}
            name={'email'}
            value={email}
            type={'text'}
            textLabel={'Email'}
            required
          />
          <StyledInput
            onChange={passwordHandler}
            error={passwordError}
            id={'password'}
            name={'password'}
            value={password}
            type={'password'}
            textLabel={'Password'}
            required
          />

          <div className="signIn">
            <button disabled={!formValid} type="button"
                    aria-label="Sign In" className="whiteButton" onClick={handleSubmit}>
              Sign In
            </button>
            {loginError ? <div style={{ color: 'red' }}>Такой пользователь не найден</div> : ''}
          </div>
          <hr />
          <div className="signUp"><Link to="/reg"><BlueButton title="Sign Up" /></Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;