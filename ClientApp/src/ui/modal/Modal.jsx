import React from 'react';
import { Modal as AntdModal } from 'antd';

export const Modal = (props) => {
  const children = props.children

  return (
    <AntdModal {...props} >
      {children}
    </AntdModal>
  );
};
