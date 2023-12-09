import React from 'react';
import styled from 'styled-components';
import { Form } from 'antd';

export const FormItem = (props) => {
  const children = props.children

  const StyledFormItem = styled(Form.Item)`
    .ant-row {
      display: block;

      .ant-form-item-label {
        text-align: start;

        label {
          &:before,
          &:after {
            display: none !important;
          }

          &.ant-form-item-required {
            &:after {
              display: inline !important;
              content: '*';
              color: red;
            }
          }
        }
      }
    }
  `;
  
  return (
    <StyledFormItem {...props} >
      {children}
    </StyledFormItem>
  );
};
