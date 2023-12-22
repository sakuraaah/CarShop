import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faCar } from '@fortawesome/free-solid-svg-icons'

export const Icon = (props) => {
  return (
    <FontAwesomeIcon 
      {...props}
      icon={props.icon ?? faCar}
    />
  )
};
