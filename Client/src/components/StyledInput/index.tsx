import React, {useState} from 'react';
import {InputWrapper, StyledLabel, PasswordWrapper, PasswordButton} from "./styles";

export const StyledInput = ({onChange, value, name, required, id, error, textLabel, type}) => {
    const [inputType, setInputType] = useState(type)
    let toggleType = () => {
    (inputType === 'password') ? setInputType('text') : setInputType('password')
}
return    (
    <InputWrapper>
        <StyledLabel htmlFor={id}>{textLabel}</StyledLabel>
        <PasswordWrapper>
        <input onChange={onChange} onBlur={onChange} value={value} id={id}
               name={name} type={inputType} required={required}/>
        {(type === 'password') ? <PasswordButton onClick={toggleType}>&#10059;</PasswordButton> : <span></span>}
        </PasswordWrapper>
        {(error && error.length > 0) && <div style={{color: 'red'}}>{error}</div>}

    </InputWrapper>

)};
