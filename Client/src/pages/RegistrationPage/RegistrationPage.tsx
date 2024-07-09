import React, {useEffect, useState} from 'react';
import {Link, useHistory} from 'react-router-dom';
import BlueButton from '../../UI/Buttons/blueButton';
import './RegistrationPage.css';
import AuthService from '../../services/api/auth.service';
import {StyledInput} from "../../components/StyledInput";


const RegistrationPage = () => {
  const history = useHistory();
  const name = React.createRef();

  const nameCheck = (e, email) => {
    if (e) {
      return e
    } else {
      return email.split('@')[0]
    }
  }
  const handleSubmit = () => {
    AuthService.register(email, nameCheck(name.current.value, email), password)
      .then((res) => {
        AuthService.login(email, password).then(() => {
          history.push("/projects")
        })
      });

  };


  const [email, setEmail] = useState();
  const [password, setPassword] = useState();
  const [emailError, setEmailError] = useState('');
  const [passwordError, setPasswordError] = useState('');
  const [formValid, setFormValid] = useState(false)

  useEffect(() => {
    if (emailError || passwordError) {
      setFormValid(false)
    } else {
      setFormValid(true)
    }
  }, [emailError, passwordError])


  const latinValid = (e, err) => {
    /* eslint-disable-next-line */
    const reg = /^[A-z\0-9\u00C0-\u00ff\s'\.,-\/#@!$%\^&\*;:{}=\-_`~()]+$/;
    if (e.target.value && !reg.test(String(e.target.value).toLowerCase())) {
      err('Только латиница!')
      return false
    } else {
      setEmailError('')
      return true
    }
  }

  const emailHandler = (e) => {
    setEmail(e.target.value);
    const reg = /\S+@\S+\.\S+/;
    if (latinValid(e, setEmailError)) {
      if (e.target.value && !reg.test(String(e.target.value).toLowerCase())) {
        setEmailError('Некорректный E-mail')
      } else if (!e.target.value) {
        setEmailError('E-mail не может быть пустым')
        setFormValid(true)
      } else {
        setEmailError('')
        return true
      }
    }
  }

  const passwordHandler = (e) => {
    setPassword(e.target.value)
    if (latinValid(e, setEmailError)) {
      if (e.target.value.length < 8) {
        setPasswordError('Пароль должен быть не менее 8 символов')
        if (!e.target.value) {
          setPasswordError('Пароль не может быть пустым')
        }
      } else {
        setPasswordError()
      }
    }
  }

  return (
    <div className="auth">
      <div className="authForm">
        {/*<Link to="/"><Logo className="logotype" /></Link>*/}
        <div className="inputWrapper">
          <StyledInput
            onChange={emailHandler}
            error={emailError}
            id={'email'}
            name={'email'}
            value={email}
            textLabel={'Email'}
            type={'text'}
            required
          />
          <StyledInput
            onChange={passwordHandler}
            error={passwordError}
            id={'password'}
            name={'password'}
            value={password}
            textLabel={'Password'}
            type={'password'}
            required
          />
          <div className="inputName">Name</div>
          <input type="text" ref={name}/>

          <div className="signUp">
            <button disabled={!formValid} type="button"
                    aria-label="Sign In" className="whiteButton" onClick={handleSubmit}>
              Sign Up
            </button>
          </div>
          <hr/>
          <div className="signIn"><Link to="/login"><BlueButton title="Sign In"/></Link></div>
        </div>
      </div>
    </div>
  );
};

export default RegistrationPage;
