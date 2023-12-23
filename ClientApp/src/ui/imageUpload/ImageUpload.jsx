import React, { useState } from 'react';
import styled from 'styled-components';
import { Image, Input, SideBySide, Switch } from '../../ui';
import { Upload } from 'antd';
import { LoadingOutlined, PlusOutlined } from '@ant-design/icons';

export const ImageUpload = ({
  form,
  imageUrl,
  setImageUrl,
  name,
  rules,
  view = false
}) => {
  const [loading, setLoading] = useState(false);
  const [isExternal, setIsExternal] = useState(false);

  const StyledImage = styled.img`
    position:relative;
    width: 100%;
    height: 100%;
    object-fit: cover;
    border-radius: 8px;
    padding: 5px;
  `; 

  const StyledInput = styled(Input)`
    &.hidden {
      display: none;
    }
  `; 

  const uploadButton = (
    <div>
      {loading ? <LoadingOutlined /> : <PlusOutlined />}
      <div style={{ marginTop: 8 }}>Upload</div>
    </div>
  );

  const handleChange = (info) => {
    if (info.file.status === 'uploading') {
      setLoading(true);
      return;
    }
    if (info.file.status === 'done') {
      setLoading(false)
      setImageUrl(info.file.response.data.value)
      form.setFieldValue(name, info.file.response.data.value)
    }
  };

  return view ? (
    <Image
      src={imageUrl}
      width={200}
      height={200}
    />
  ) : (
    <SideBySide
      left={
        isExternal ? (
          <Image
            src={imageUrl}
            width={200}
            height={200}
          />
        ) : (
          <Upload
            name="imgSrc"
            listType="picture-card"
            className="picture-uploader"
            showUploadList={false}
            action="api/image"
            maxCount={1}
            onChange={handleChange}
          >
            {imageUrl ? (
              <StyledImage 
                src={imageUrl}
                alt="picture"
              />
            ) : (
              uploadButton
            )}
          </Upload>
        )
      }
      right={
        <>
          <Switch
            label={'Is external image'}
            name={'isExternal'}
            onChange={(e) => {
              setIsExternal(e)
              form.setFieldValue(name, null)
              setImageUrl(null)
            }}
          />
          <StyledInput
            className={`${isExternal ? '' : 'hidden'}`}
            name={name}
            label={'Image Url'}
            rules={rules}
            onBlur={(e) => setImageUrl(e.target.value)}
          />
        </>
      }
    />
  )
}
