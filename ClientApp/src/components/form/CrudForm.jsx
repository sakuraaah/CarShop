import React from 'react';
import { useParams } from 'react-router-dom';
import { 
  Button, 
  Form, 
  Label,
  LabelFormItem,
} from '../../ui';
import { 
  ButtonList,
  FormHeader,
  StyledPage, 
  StyledWrapper,
} from '../../styles/layout/form';
import useQueryApiClient from '../../utils/useQueryApiClient';

export const CrudForm = ({
  form,
  url,
  label,
  children
}) => {
  const { id } = useParams();

  const onSubmit = async (updateStatus, status = 'Confirmed') => {
    try {
      const values = await form.validateFields()

      if (!id) {
        createPost(values)
      }
    } catch (errorInfo) {
      form.scrollToField(errorInfo.errorFields[0]?.name, { behavior: 'smooth', block: 'center', scrollMode: 'if-needed' })
    }
  }

  const { appendData: createPost, isLoading: createLoading } = useQueryApiClient({
    request: {
      url: url,
      method: 'POST'
    }
  });

  return (
    <StyledPage>
      <Form form={form} >
        <FormHeader>
          <Label label={label} extraBold />
          <LabelFormItem 
            label={'Status'} 
            labelValue={''}
          />
        </FormHeader>
        
        {children}

        <StyledWrapper>
          <ButtonList>
            <Button 
              htmlType="submit" 
              onClick={() => onSubmit(false)} 
              type="primary" 
              label={'Save'} 
            />
            <Button 
              htmlType="submit" 
              onClick={() => onSubmit(false)} 
              label={'Submit'} 
            />
          </ButtonList>
        </StyledWrapper>
      </Form>
    </StyledPage>
  )
}
