import React from 'react';
import { Avatar as AntdAvatar } from 'antd';
import { faUser } from '@fortawesome/free-solid-svg-icons'
import { Icon } from '../../ui'

export const Avatar = (props) => {
  return (
    <AntdAvatar 
      {...props}
      icon={<Icon icon={faUser}/>}
    />
  )
}
